using System;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    internal class EmptyReadOnlyBinaryDataAccessor : IReadOnlyBinaryDataAccessor
    {
        public EmptyReadOnlyBinaryDataAccessor(long length)
        {
            this.Length = length;
        }

        public long Length { get; set; }
        public long Position { get; set; }

        public byte[] ReadArray()
        {
            return new byte[Length];
        }

        public byte[] ReadArray(long index, int length)
        {
            return new byte[length];
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
            return 0;
        }

        public Task<byte> ReadByteAsync(long index)
        {
            return Task.FromResult(ReadByte(index));
        }

        public ReadOnlyMemory<byte> ReadMemory()
        {
            return ReadArray();
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync()
        {
            return Task.FromResult(ReadMemory());
        }

        public ReadOnlyMemory<byte> ReadMemory(long index, int length)
        {
            return ReadArray(index, length);
        }

        public Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length)
        {
            return Task.FromResult(ReadMemory(index, length));
        }

        public ReadOnlySpan<byte> ReadSpan()
        {
            return ReadArray();
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {
            return ReadArray(index, length);
        }
    }
}
