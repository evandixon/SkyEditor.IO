using System;
using System.IO.MemoryMappedFiles;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    internal class MemoryMappedFileDataAccessor : IBinaryDataAccessor
    {
        public MemoryMappedFileDataAccessor(MemoryMappedFile memoryMappedFile, long maxLength)
        {
            Length = maxLength;
            _file = memoryMappedFile ?? throw new ArgumentNullException(nameof(memoryMappedFile));
        }

        private readonly MemoryMappedFile _file;

        public long Length { get; }

        public byte[] ReadArray()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return ReadArray(0, (int)Length);
        }

        public byte[] ReadArray(long index, int length)
        {
            var buffer = new byte[length];
            using (var accessor = _file.CreateViewAccessor(index, length))
            {
                accessor.ReadArray(0, buffer, 0, length);
            }            
            return buffer;
        }

        public Task<byte[]> ReadArrayAsync()
        {
            return Task.FromResult(ReadArray());
        }

        public Task<byte[]> ReadArrayAsync(long index, int length)
        {
            return Task.FromResult(ReadArray(index, length));
        }

        public byte ReadByte(long index)
        {
            using var accessor = _file.CreateViewAccessor(index, 1);
            return accessor.ReadByte(0);
        }

        public Task<byte> ReadByteAsync(long index)
        {
            return Task.FromResult(ReadByte(index));
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync()
        {
            return Task.FromResult((ReadOnlyMemory<byte>)ReadArray().AsMemory());
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length)
        {
            return Task.FromResult((ReadOnlyMemory<byte>)ReadArray(index, length).AsMemory());
        }

        public ReadOnlySpan<byte> ReadSpan()
        {
            return (ReadOnlySpan<byte>)ReadArray().AsSpan();
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {
            return (ReadOnlySpan<byte>)ReadArray(index, length).AsSpan();
        }

        public void Write(byte[] value)
        {
            using var accessor = _file.CreateViewAccessor();
            accessor.WriteArray(0, value, 0, value.Length);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            Write(value.ToArray());
        }

        public void Write(long index, byte value)
        {
            using var accessor = _file.CreateViewAccessor(index, 1);
            accessor.Write(0, value);
        }

        public void Write(long index, int length, byte[] value)
        {
            using var accessor = _file.CreateViewAccessor(index, length);
            accessor.WriteArray(0, value, 0, length);
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            Write(index, length, value.ToArray());
        }

        public Task WriteAsync(byte[] value)
        {
            Write(value);
            return Task.CompletedTask;
        }

        public Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            Write(value.ToArray());
            return Task.CompletedTask;
        }

        public Task WriteAsync(long index, byte value)
        {
            Write(index, value);
            return Task.CompletedTask;
        }

        public Task WriteAsync(long index, int length, byte[] value)
        {
            Write(index, length, value);
            return Task.CompletedTask;
        }

        public Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            Write(index, length, value.ToArray());
            return Task.CompletedTask;
        }
     }
}
