using SkyEditor.IO.Binary.Internal;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
{
    public class BinaryFile : IBinaryDataAccessor
    {
        public BinaryFile(string filename, IFileSystem fileSystem)
        {
            Initialize(fileSystem.ReadAllBytes(filename));
        }

        public BinaryFile(string filename) : this(filename, PhysicalFileSystem.Instance)
        {
        }

        public BinaryFile(byte[] rawData)
        {
            Initialize(rawData);
        }

        private void Initialize(byte[] rawData)
        {
            Accessor = new InMemoryBinaryDataAccessor(rawData);
        }

        private IBinaryDataAccessor Accessor { get; set; }

        public long Length => Accessor.Length;

        public byte[] ReadArray() => Accessor.ReadArray();

        public byte[] ReadArray(long index, int length) => Accessor.ReadArray(index, length);

        public Task<byte[]> ReadArrayAsync() => Accessor.ReadArrayAsync();

        public Task<byte[]> ReadArrayAsync(long index, int length) => Accessor.ReadArrayAsync(index, length);

        public byte ReadByte(long index) => Accessor.ReadByte(index);

        public Task<byte> ReadByteAsync(long index) => Accessor.ReadByteAsync(index);

        public ReadOnlyMemory<byte> ReadMemory() => Accessor.ReadMemory();

        public ReadOnlyMemory<byte> ReadMemory(long index, int length) => Accessor.ReadMemory(index, length);

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

        public void Write(ReadOnlyMemory<byte> value) => Accessor.Write(value);

        public void Write(ReadOnlySpan<byte> value) => Accessor.Write(value);

        public void Write(long index, int length, ReadOnlyMemory<byte> value) => Accessor.Write(index, length, value);

        public void Write(long index, int length, ReadOnlySpan<byte> value) => Accessor.Write(index, length, value);

        public Task WriteAsync(ReadOnlyMemory<byte> value) => Accessor.WriteAsync(value);

        public Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value) => Accessor.WriteAsync(index, length, value);
    }
}
