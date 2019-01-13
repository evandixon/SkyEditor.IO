using SkyEditor.IO.Binary.Internal;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        private BinaryFile(Stream stream)
        {
            Accessor = new StreamBinaryDataAccessor(stream);
        }

        private void OpenAutodetectFormat(string filename, IFileSystem fileSystem)
        {
            byte[] byteArray;
            try
            {
                byteArray = fileSystem.ReadAllBytes(filename);
            }
            catch (OutOfMemoryException)
            {
                // The file's too large to fit into memory as an array.
                // We need to instead use a less memory-intensive container.
                byteArray = null;
            }

            if (byteArray != null)
            {
                Accessor = new InMemoryBinaryDataAccessor(byteArray);
            }
            else
            {
                DisposeStream();
                SourceStream = fileSystem.OpenFile(filename);
                Accessor = new StreamBinaryDataAccessor(SourceStream);
            }
        }

        private IBinaryDataAccessor Accessor { get; set; }

        private string Filename { get; set; }

        /// <summary>
        /// The underlying file system from which data is retrieved.
        /// </summary>
        /// <remarks>
        /// If this has a value, then this class has free reign over how data is accessed, including opening and closing all required resources.
        /// If this is null, then this class is not allowed to close the <see cref="Accessor"/> or its resources, since this class cannot reopen it afterward.
        /// </remarks>
        private IFileSystem FileSystem { get; set; }

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

            DisposeStream();

            using (var stream = FileSystem.OpenFile(Filename))
            {
                stream.SetLength(length);
            }

            OpenAutodetectFormat(Filename, FileSystem);
        }

        public void Dispose()
        {
            DisposeStream();
        }

        private void DisposeStream()
        {
            if (SourceStream != null && FileSystem != null)
            {
                SourceStream.Dispose();
                SourceStream = null;
            }
        }
    }
}
