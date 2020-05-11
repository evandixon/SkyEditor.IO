using System;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    internal class ArrayBinaryDataAccessor : IVariableLengthBinaryDataAccessor
    {
        public ArrayBinaryDataAccessor(byte[] rawData)
        {
            _rawData = rawData ?? throw new ArgumentNullException(nameof(rawData));
        }

        private byte[] _rawData;

        public long Length => _rawData.LongLength;
        public long Position { get; set; }

        public byte[] ReadArray()
        {
            return _rawData;
        }

        public byte[] ReadArray(long index, int length)
        {
            return _rawData.AsSpan().Slice((int)index, length).ToArray();
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
            return _rawData[index];
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
                return _rawData.AsMemory().Slice((int)index, length);
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
            return _rawData.AsSpan();
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {           
            if (index < int.MaxValue)
            {
                return _rawData.AsSpan().Slice((int)index, length);
            }
            else
            {
                return ReadArray(index, length);
            }
        }

        public void Write(byte[] value)
        {
            value.CopyTo(_rawData, 0);
        }

        public void Write(ReadOnlyMemory<byte> value)
        {
            value.CopyTo(_rawData);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            value.CopyTo(_rawData);
        }

        public void Write(long index, byte value)
        {
            _rawData[index] = value;
        }

        public void Write(long index, int length, byte[] value)
        {
            var toCopy = value.AsMemory().Slice(0, length);
            if (index < int.MaxValue)
            {
                toCopy.CopyTo(_rawData.AsMemory().Slice((int)index, length));
            }
            else
            {
                toCopy.ToArray().CopyTo(_rawData, index);
            }
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            var toCopy = value.Slice(0, length);
            if (index < int.MaxValue)
            {
                toCopy.CopyTo(_rawData.AsSpan().Slice((int)index, length));
            }
            else
            {
                toCopy.ToArray().CopyTo(_rawData, index);
            }
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
            return new MemoryBinaryDataAccessor(_rawData.AsMemory().Slice((int)index, (int)length));
        }

        public void SetLength(long length)
        {
            if (length > int.MaxValue)
            {
                throw new NotSupportedException(Properties.Resources.BinaryFile_SetLength_SizeToLarge);
            }

            Array.Resize(ref _rawData, (int)length);
        }
    }
}
