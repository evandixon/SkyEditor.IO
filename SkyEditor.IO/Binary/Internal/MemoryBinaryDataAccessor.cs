using System;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    internal class MemoryBinaryDataAccessor : IBinaryDataAccessor
    {
        public MemoryBinaryDataAccessor(Memory<byte> rawData)
        {
            _rawData = rawData;
        }

        private readonly Memory<byte> _rawData;

        public long Length => _rawData.Length;
        public long Position { get; set; }

        public byte[] ReadArray()
        {
            return _rawData.ToArray();
        }

        public byte[] ReadArray(long index, int length)
        {
            return _rawData.Slice((int)index, length).ToArray();
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
            return _rawData.Span[(int)index];
        }

        public Task<byte> ReadByteAsync(long index)
        {
            return Task.FromResult(ReadByte(index));
        }

        public ReadOnlyMemory<byte> ReadMemory()
        {
            return _rawData;
        }

        public ReadOnlyMemory<byte> ReadMemory(long index, int length)
        {
            if (index < int.MaxValue)
            {
                return _rawData.Slice((int)index, length);
            }
            else
            {
                return ReadArray(index, length);
            }
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync()
        {
            return Task.FromResult(ReadMemory());
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length)
        {
            return Task.FromResult(ReadMemory(index, length));
        }

        public ReadOnlySpan<byte> ReadSpan()
        {
            return _rawData.Span;
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {
            if (index < int.MaxValue)
            {
                return _rawData.Span.Slice((int)index, length);
            }
            else
            {
                return ReadArray(index, length);
            }
        }

        public void Write(byte[] value)
        {
            value.CopyTo(_rawData);
        }

        public void Write(ReadOnlyMemory<byte> value)
        {
            value.CopyTo(_rawData);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            value.CopyTo(_rawData.Span);
        }

        public void Write(long index, byte value)
        {
            _rawData.Span[(int)index] = value;
        }

        public void Write(long index, int length, byte[] value)
        {
            var toCopy = value.AsMemory().Slice(0, length);
            toCopy.CopyTo(_rawData.Slice((int)index, length));
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            var toCopy = value.Slice(0, length);
            toCopy.CopyTo(_rawData.Span.Slice((int)index, length));
        }

        public Task WriteAsync(byte[] value)
        {
            Write(value);
            return Task.CompletedTask;
        }

        public Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            Write(value);
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
            WriteAsync(index, length, value);
            return Task.CompletedTask;
        }

        public Task WriteAsync(long index, int length, ReadOnlySpan<byte> value)
        {
            WriteAsync(index, length, value);
            return Task.CompletedTask;
        }

        public IBinaryDataAccessor Slice(long index, long length)
        {
            return new MemoryBinaryDataAccessor(_rawData.Slice((int)index, (int)length));
        }
    }
}
