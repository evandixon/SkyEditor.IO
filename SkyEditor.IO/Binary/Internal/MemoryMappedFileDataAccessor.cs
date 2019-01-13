using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    public class MemoryMappedFileDataAccessor : IBinaryDataAccessor
    {
        public MemoryMappedFileDataAccessor(MemoryMappedFile memoryMappedFile, long maxLength)
        {
            Length = maxLength;
            _file = memoryMappedFile ?? throw new ArgumentNullException(nameof(memoryMappedFile));
        }

        private MemoryMappedFile _file;

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
            _file.CreateViewAccessor(index, length).ReadArray(0, buffer, 0, length);
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
            return _file.CreateViewAccessor(index, 1).ReadByte(0);
        }

        public Task<byte> ReadByteAsync(long index)
        {
            return Task.FromResult(ReadByte(index));
        }

#if ENABLE_SPAN_AND_MEMORY
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
#endif

        public void Write(byte[] value)
        {
            _file.CreateViewAccessor().WriteArray(0, value, 0, value.Length);
        }

#if ENABLE_SPAN_AND_MEMORY
        public void Write(ReadOnlySpan<byte> value)
        {
            Write(value.ToArray());
        }
#endif

        public void Write(long index, byte value)
        {
            _file.CreateViewAccessor(index, 1).Write(0, value);
        }

        public void Write(long index, int length, byte[] value)
        {
            _file.CreateViewAccessor(index, length).WriteArray(0, value, 0, length);
        }

#if ENABLE_SPAN_AND_MEMORY
        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            Write(index, length, value.ToArray());
        }
#endif

        public Task WriteAsync(byte[] value)
        {
            Write(value);
            return Task.CompletedTask;
        }

#if ENABLE_SPAN_AND_MEMORY
        public Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            Write(value.ToArray());
            return Task.CompletedTask;
        }
#endif

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

#if ENABLE_SPAN_AND_MEMORY
        public Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            Write(index, length, value.ToArray());
            return Task.CompletedTask;
        }
#endif
    }
}
