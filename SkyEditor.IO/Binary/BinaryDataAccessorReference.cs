using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO
{
    /// <summary>
    /// Provides a view to a subset of a <see cref="IBinaryDataAccessor"/> or other <see cref="BinaryDataAccessorReference"/>
    /// </summary>
    public class BinaryDataAccessorReference : IBinaryDataAccessor
    {
        public BinaryDataAccessorReference(IBinaryDataAccessor data, long offset, long length)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Data = data ?? throw new ArgumentNullException(nameof(data));
            Offset = offset;
            Length = length;
        }

        public BinaryDataAccessorReference(BinaryDataAccessorReference reference, long offset, long length)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            Data = reference.Data;
            Offset = reference.Offset + offset;
            Length = length;
        }

        private IBinaryDataAccessor Data { get; }

        private long Offset { get; set; }

        public long Length { get; private set; }

        public byte[] ReadArray()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return Data.ReadArray(Offset, (int)Length);
        }

        public ReadOnlyMemory<byte> ReadMemory()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return Data.ReadMemory(Offset, (int)Length);
        }

        public ReadOnlySpan<byte> ReadSpan()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return Data.ReadSpan(Offset, (int)Length);
        }

        public async Task<byte[]> ReadArrayAsync()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return await Data.ReadArrayAsync(Offset, (int)Length);
        }

        public async Task<ReadOnlyMemory<byte>> ReadMemoryAsync()
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            return await Data.ReadMemoryAsync(Offset, (int)Length);
        }

        public byte ReadByte(long index)
        {
            return Data.ReadByte(Offset + index);
        }

        public async Task<byte> ReadByteAsync(long index)
        {
            return await Data.ReadByteAsync(Offset + index);
        }

        public byte[] ReadArray(long index, int length)
        {
            return Data.ReadArray(Offset + index, (int)Math.Min(Length, length));
        }

        public ReadOnlyMemory<byte> ReadMemory(long index, int length)
        {
            return Data.ReadMemory(Offset + index, (int)Math.Min(Length, length));
        }

        public ReadOnlySpan<byte> ReadSpan(long index, int length)
        {
            return Data.ReadSpan(Offset + index, (int)Math.Min(Length, length));
        }

        public async Task<byte[]> ReadArrayAsync(long index, int length)
        {
            return await Data.ReadArrayAsync(Offset + index, (int)Math.Min(Length, length));
        }

        public async Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length)
        {
            return await Data.ReadMemoryAsync(Offset + index, (int)Math.Min(Length, length));
        }

        public void Write(byte[] value)
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            Data.Write(Offset, (int)Length, value);
        }

        public void Write(ReadOnlyMemory<byte> value)
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            Data.Write(Offset, (int)Length, value);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            Data.Write(Offset, (int)Length, value);
        }

        public async Task WriteAsync(byte[] value)
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            await Data.WriteAsync(Offset, (int)Length, value);
        }

        public async Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            if (Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.Binary_ErrorLengthTooLarge);
            }

            await Data.WriteAsync(Offset, (int)Length, value);
        }

        public void Write(long index, byte value)
        {
            Data.Write(Offset + index, value);
        }

        public void Write(long index, int length, byte[] value)
        {
            Data.Write(Offset + index, (int)Math.Min(length, Length), value);
        }

        public void Write(long index, int length, ReadOnlyMemory<byte> value)
        {
            Data.Write(Offset + index, (int)Math.Min(length, Length), value);
        }

        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            Data.Write(Offset + index, (int)Math.Min(length, Length), value);
        }

        public async Task WriteAsync(long index, byte value)
        {
            await Data.WriteAsync(Offset + index, value);
        }

        public async Task WriteAsync(long index, int length, byte[] value)
        {
            await Data.WriteAsync(Offset + index, (int)Math.Min(length, Length), value);
        }

        public async Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            await Data.WriteAsync(Offset + index, (int)Math.Min(length, Length), value);
        }
    }

    public static class IBinaryDataAccessorExtensions
    {
        /// <summary>
        /// Gets a view on top of the current data
        /// </summary>
        /// <param name="data">Data to reference</param>
        /// <param name="offset">Offset of the view</param>
        /// <param name="length">Maximum length of the view</param>
        /// <returns>A view on top of the data</returns>
        public static BinaryDataAccessorReference GetDataReference(this IBinaryDataAccessor data, long offset, long length)
        {
            return new BinaryDataAccessorReference(data, offset, length);
        }
    }
}