using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary.Internal
{
    public class StreamBinaryDataAccessor : IBinaryDataAccessor
    {
        public StreamBinaryDataAccessor(Stream stream)
        {
            this.SourceStream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// The underlying data store which the current <see cref="IBinaryDataAccessor"/> interacts with
        /// </summary>
        public Stream SourceStream { get; set; }

        /// <summary>
        /// A lock used to ensure thread safety when interacting with <see cref="SourceStream"/>
        /// </summary>
        /// <remarks>
        /// Because the concurrent request count is 1, this is functionally no different from a monitor.
        /// The reason this class was chosen over a lock or a monitor was because this has better async support
        /// </remarks>
        private SemaphoreSlim StreamSemaphore { get; } = new SemaphoreSlim(1);

        public long Length => SourceStream.Length;

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
            try
            {
                StreamSemaphore.Wait();

                byte[] buffer = new byte[length];
                SourceStream.Seek(index, SeekOrigin.Begin);
                SourceStream.Read(buffer, 0, length);
                return buffer;
            }
            finally
            {
                StreamSemaphore.Release();
            }            
        }

        public async Task<byte[]> ReadArrayAsync()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return await ReadArrayAsync(0, (int)Length);
        }

        public async Task<byte[]> ReadArrayAsync(long index, int length)
        {
            try
            {
                await StreamSemaphore.WaitAsync();

                byte[] buffer = new byte[length];
                SourceStream.Seek(index, SeekOrigin.Begin);
                await SourceStream.ReadAsync(buffer, 0, length);
                return buffer;
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public byte ReadByte(long index)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(index, SeekOrigin.Begin);
                return (byte)SourceStream.ReadByte();
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task<byte> ReadByteAsync(long index)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(index, SeekOrigin.Begin);
                return (byte)SourceStream.ReadByte();
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task<ReadOnlyMemory<byte>> ReadMemoryAsync()
        {
            return await ReadArrayAsync();
        }

        public async Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length)
        {
            return await ReadArrayAsync(index, length);
        }

        public ReadOnlySpan<byte> ReadSpan()
        {
            return ReadArray();
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {
            return ReadArray(index, length);
        }

        public void Write(byte[] value)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(0, SeekOrigin.Begin);
                SourceStream.Write(value, 0, value.Length);
            }
            finally
            {
                StreamSemaphore.Release();
            }            
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(0, SeekOrigin.Begin);
                SourceStream.Write(value);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public void Write(long index, byte value)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(index, SeekOrigin.Begin);
                SourceStream.WriteByte(value);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public void Write(long index, int length, byte[] value)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(index, SeekOrigin.Begin);
                SourceStream.Write(value, 0, length);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            try
            {
                StreamSemaphore.Wait();
                SourceStream.Seek(index, SeekOrigin.Begin);
                SourceStream.Write(value.Slice(0, length));
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task WriteAsync(byte[] value)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(0, SeekOrigin.Begin);
                await SourceStream.WriteAsync(value, 0, value.Length);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(0, SeekOrigin.Begin);
                await SourceStream.WriteAsync(value);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task WriteAsync(long index, byte value)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(index, SeekOrigin.Begin);
                SourceStream.WriteByte(value);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task WriteAsync(long index, int length, byte[] value)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(index, SeekOrigin.Begin);
                await SourceStream.WriteAsync(value, 0, length);
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }

        public async Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            try
            {
                await StreamSemaphore.WaitAsync();
                SourceStream.Seek(index, SeekOrigin.Begin);
                await SourceStream.WriteAsync(value.Slice(0, length));
            }
            finally
            {
                StreamSemaphore.Release();
            }
        }
    }
}
