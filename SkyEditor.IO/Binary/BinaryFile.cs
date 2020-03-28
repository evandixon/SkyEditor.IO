using SkyEditor.IO.Binary.Internal;
using SkyEditor.IO.FileSystem;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
{
    public class BinaryFile : IBinaryDataAccessor, IDisposable
    {
        public BinaryFile(string filename, IFileSystem fileSystem)
        {
            Filename = filename;
            FileSystem = fileSystem;
            OpenAutodetectFormat(filename, fileSystem);
        }

        public BinaryFile(string filename) : this(filename, PhysicalFileSystem.Instance)
        {
        }

        public BinaryFile(byte[] rawData)
        {
            Accessor = new InMemoryBinaryDataAccessor(rawData);
        }

        public BinaryFile(MemoryMappedFile memoryMappedFile, int fileLength)
        {
            Accessor = new MemoryMappedFileDataAccessor(memoryMappedFile, fileLength);
        }

        public BinaryFile(Stream stream)
        {
            Accessor = new StreamBinaryDataAccessor(stream);
        }

        private void OpenAutodetectFormat(string filename, IFileSystem fileSystem)
        {
            CloseFileResources(); // Resources should already be null, but if this isn't called, we may leave a file open accidentally. Luckily it's safe to call this multiple times.

            var fileLength = fileSystem.GetFileLength(filename);

            // First try a byte array
            // It's the fastest, but has the biggest memory footprint
            // We might not be able to fit a large file into memory, and I don't know how to open a 2GB or larger file in memory,
            // so we may have to fall back onto something else
            if (fileLength < int.MaxValue)
            {
                byte[] byteArray;
                try
                {
                    byteArray = fileSystem.ReadAllBytes(filename);
                }
                catch (OutOfMemoryException)
                {
                    byteArray = null;
                }

                if (byteArray != null)
                {
                    Accessor = new InMemoryBinaryDataAccessor(byteArray);
                    return;
                }
            }

            // Next, try a memory mapped file system
            // Its speed is comparable to a byte array, but slightly slower (difference may be bigger since the InMemoryBinaryDataAccessor is the only one to support Span and Memory, which I have not yet seen the speed of first-hand)
            // This method might not be available depending on whether or not we have a real file system at our disposal
            // In the future, this may need to be feature-switchable, since we can't resize this file without disposing of it first
            if (fileSystem is IMemoryMappableFileSystem memoryMappableFileSystem)
            {
                MemoryMappedFile = memoryMappableFileSystem.OpenMemoryMappedFile(filename);
                Accessor = new MemoryMappedFileDataAccessor(MemoryMappedFile, fileLength);
                try
                {
                    // Sometimes we might not have enough memory.
                    // When that happens, we get an IOException saying something "There are not enough memory resources available"
                    if (MemoryMappedFile.CreateViewAccessor().Capacity > -1) // Compare the capacity to -1 just to see if we get an IOException
                    {
                        return;
                    }
                }
                catch (IOException)
                {
                    // We can't use a MemoryMapped file.
                    // Streams are more reliable anyway; just slower.
                }
            }

            // If all else fails, we can use a stream.    
            // It's the slowest since it is not thread-safe, and the StreamBinaryDataAccessor has to use appropriate locking
            SourceStream = fileSystem.OpenFile(filename);
            Accessor = new StreamBinaryDataAccessor(SourceStream);
        }

        public async Task Save(string filename, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            switch (Accessor)
            {
                case InMemoryBinaryDataAccessor inMemoryAccessor:
                    fileSystem.WriteAllBytes(filename, inMemoryAccessor.ReadArray());
                    break;
                case MemoryMappedFileDataAccessor _:
                    if (this.Filename == filename)
                    {
                        // Trying to save to the current file
                        // To-do: determine if the file flushes automatically

                        break;
                    }

                    var memoryMappedStreamView = MemoryMappedFile.CreateViewStream();
                    using (var dest = fileSystem.OpenFileWriteOnly(filename))
                    {
                        await memoryMappedStreamView.CopyToAsync(dest);
                    }
                    break;
                case StreamBinaryDataAccessor streamAccessor:
                    var source = streamAccessor.SourceStream;
                    if (this.Filename == filename)
                    {
                        // Trying to save to the current file
                        await source.FlushAsync();
                        break;
                    }
                    
                    source.Seek(0, SeekOrigin.Begin);
                    using (var dest = fileSystem.OpenFileWriteOnly(filename))
                    {
                        await source.CopyToAsync(dest);
                    }
                    break;
                default:
                    throw new NotImplementedException("Unsupported IBinaryDataAccessor type: " + Accessor.GetType().Name);
            }
        }

        public async Task Save()
        {
            if (string.IsNullOrEmpty(Filename) || FileSystem == null)
            {
                throw new InvalidOperationException(Properties.Resources.BinaryFile_ErrorSavedWithoutFilenameOrFilesystem);
            }

            await Save(Filename, FileSystem);
        }

        private IBinaryDataAccessor Accessor { get; set; }

        public string Filename { get; protected set; }

        /// <summary>
        /// The underlying file system from which data is retrieved.
        /// </summary>
        /// <remarks>
        /// If this has a value, then this class has free reign over how data is accessed, including opening and closing all required resources.
        /// If this is null, then this class is not allowed to close the <see cref="Accessor"/> or its resources, since this class cannot reopen it afterward.
        /// </remarks>
        private IFileSystem FileSystem { get; set; }

        /// <summary>
        /// The underlying memory mapped file, if applicable
        /// </summary>
        private MemoryMappedFile MemoryMappedFile { get; set; }

        /// <summary>
        /// The underlying data source if the binary file is being accessed as a stream.
        /// </summary>
        /// <remarks>
        /// This will only have a value if the stream belongs to this class and.
        /// If the underlying data source is a stream that is not owned by this class, <see cref="SourceStream"/> will be null.
        /// This stream will be disposed along with the class if set.
        /// </remarks>
        private Stream SourceStream { get; set; }

        public long Length => Accessor.Length;

        public byte[] ReadArray() => Accessor.ReadArray();

        public byte[] ReadArray(long index, int length) => Accessor.ReadArray(index, length);

        public Task<byte[]> ReadArrayAsync() => Accessor.ReadArrayAsync();

        public Task<byte[]> ReadArrayAsync(long index, int length) => Accessor.ReadArrayAsync(index, length);

        public byte ReadByte(long index) => Accessor.ReadByte(index);

        public Task<byte> ReadByteAsync(long index) => Accessor.ReadByteAsync(index);

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync() => Accessor.ReadMemoryAsync();

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length) => Accessor.ReadMemoryAsync(index, length);

        public ReadOnlySpan<byte> ReadSpan() => Accessor.ReadSpan();

        public ReadOnlySpan<byte> ReadSpan(long index, int length) => Accessor.ReadSpan(index, length);

        public void Write(byte[] value) => Accessor.Write(value);

        public void Write(long index, byte value) => Accessor.Write(index, value);

        public void Write(long index, int length, byte[] value) => Accessor.Write(index, length, value);

        public Task WriteAsync(byte[] value) => Accessor.WriteAsync(value);

        public Task WriteAsync(long index, byte value) => Accessor.WriteAsync(index, value);

        public Task WriteAsync(long index, int length, byte[] value) => Accessor.WriteAsync(index, length, value);

        public void Write(ReadOnlySpan<byte> value) => Accessor.Write(value);

        public void Write(long index, int length, ReadOnlySpan<byte> value) => Accessor.Write(index, length, value);

        public Task WriteAsync(ReadOnlyMemory<byte> value) => Accessor.WriteAsync(value);

        public Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value) => Accessor.WriteAsync(index, length, value);
         
        /// <summary>
        /// Resizes the file. This method is NOT thread safe
        /// </summary>
        /// <param name="length">New size of the file</param>
        /// <exception cref="InvalidOperationException">Thrown when the underlying data source cannot be resized to the requested size and the class is not allowed to close the file.</exception>
        public void SetLength(long length)
        {
            if (Accessor is IVariableLengthBinaryDataAccessor variableLengthAccessor)
            {
                try
                {
                    variableLengthAccessor.SetLength(length);
                }
                catch (NotSupportedException)
                {
                    // We can't use the current data source for a file of this length.
                    ResizeAndReopen(length);
                }
            }
            else
            {
                // The accessor doesn't support resizing at all (like memmory mapped files)
                ResizeAndReopen(length);
            }            
        }

        private void ResizeAndReopen(long length)
        {
            if (FileSystem == null)
            {
                throw new InvalidOperationException(Properties.Resources.BinaryFile_NotAllowedToCloseFile);
            }

            CloseFileResources();

            using (var stream = FileSystem.OpenFile(Filename))
            {
                stream.SetLength(length);
            }

            OpenAutodetectFormat(Filename, FileSystem);
        }

        public virtual void Dispose()
        {
            CloseFileResources();
        }

        private void CloseFileResources()
        {
            if (FileSystem != null)
            {
                if (MemoryMappedFile != null)
                {
                    MemoryMappedFile.Dispose();
                    MemoryMappedFile = null;
                }
                if (SourceStream != null)
                {
                    SourceStream.Dispose();
                    SourceStream = null;
                }
            }
        }
    }
}
