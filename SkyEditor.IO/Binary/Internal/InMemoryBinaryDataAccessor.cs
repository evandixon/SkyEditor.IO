using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    internal class InMemoryBinaryDataAccessor : IBinaryDataAccessor
    {
        public InMemoryBinaryDataAccessor(byte[] rawData)
        {
            RawData = rawData ?? throw new ArgumentNullException(nameof(rawData));
        }

        private byte[] RawData { get; set; }

        public long Length => RawData.LongLength;

        public byte[] ReadArray()
        {
            return RawData;
        }

        public byte[] ReadArray(long index, int length)
        {
            var output = new byte[length];
            Array.Copy(RawData, index, output, 0, length);
            return output;
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
            return RawData[index];
        }

        public Task<byte> ReadByteAsync(long index)
        {
            return Task.FromResult(ReadByte(index));
        }

        public ReadOnlyMemory<byte> ReadMemory()
        {
            return RawData;
        }

        public ReadOnlyMemory<byte> ReadMemory(long index, int length)
        {
            if (index < int.MaxValue)
            {
                return RawData.AsMemory().Slice((int)index, length);
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
            return RawData.AsSpan();
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {           
            if (index < int.MaxValue)
            {
                return RawData.AsSpan().Slice((int)index, length);
            }
            else
            {
                return ReadArray(index, length);
            }
        }

        public void Write(byte[] value)
        {
            value.CopyTo(RawData, 0);
        }

        public void Write(ReadOnlyMemory<byte> value)
        {
            value.CopyTo(RawData);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            value.CopyTo(RawData);
        }

        public void Write(long index, byte value)
        {
            RawData[index] = value;
        }

        public void Write(long index, int length, byte[] value)
        {
            var toCopy = value.AsMemory().Slice(0, length);
            if (index < int.MaxValue)
            {
                toCopy.CopyTo(RawData.AsMemory().Slice((int)index, length));
            }
            else
            {
                toCopy.ToArray().CopyTo(RawData, index);
            }
        }

        public void Write(long index, int length, ReadOnlyMemory<byte> value)
        {
            var toCopy = value.Slice(0, length);
            if (index < int.MaxValue)
            {
                toCopy.CopyTo(RawData.AsMemory().Slice((int)index, length));
            }
            else
            {
                toCopy.ToArray().CopyTo(RawData, index);
            }
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            var toCopy = value.Slice(0, length);
            if (index < int.MaxValue)
            {
                toCopy.CopyTo(RawData.AsSpan().Slice((int)index, length));
            }
            else
            {
                toCopy.ToArray().CopyTo(RawData, index);
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
    }
}
