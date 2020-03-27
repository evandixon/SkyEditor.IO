using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
{
    /// <summary>
    /// Provides a view to a subset of a <see cref="IWriteOnlyBinaryDataAccessor"/> or other <see cref="WriteOnlyBinaryDataAccessorReference"/>
    /// </summary>
    public class WriteOnlyBinaryDataAccessorReference : IWriteOnlyBinaryDataAccessor
    {
        public WriteOnlyBinaryDataAccessorReference(IWriteOnlyBinaryDataAccessor data, long offset, long length)
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

        public WriteOnlyBinaryDataAccessorReference(WriteOnlyBinaryDataAccessorReference reference, long offset, long length)
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


        private IWriteOnlyBinaryDataAccessor Data { get; }

        private long Offset { get; set; }

        public long Length { get; private set; }

        public void Write(byte[] value)
        {
            if (value.Length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            Data.Write(Offset, value);
        }

        public void Write(ReadOnlySpan<byte> value)
        {
            if (value.Length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            Data.Write(Offset, value.Length, value);
        }

        public async Task WriteAsync(byte[] value)
        {
            if (value.Length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            await Data.WriteAsync(Offset, value);
        }


        public async Task WriteAsync(ReadOnlyMemory<byte> value)
        {
            if (value.Length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            await Data.WriteAsync(Offset, value.Length, value);
        }

        public void Write(long index, byte value)
        {
            if (index > Length)
            {
                throw new IndexOutOfRangeException();
            }

            Data.Write(Offset + index, value);
        }

        public async Task WriteAsync(long index, byte value)
        {
            if (index > Length)
            {
                throw new IndexOutOfRangeException();
            }

            await Data.WriteAsync(Offset + index, value);
        }

        public void Write(long index, int length, byte[] value)
        {
            if (length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            Data.Write(Offset + index, length, value);
        }


        public void Write(long index, int length, ReadOnlySpan<byte> value)
        {
            if (length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            Data.Write(Offset + index, length, value);
        }

        public async Task WriteAsync(long index, int length, byte[] value)
        {
            if (length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            await Data.WriteAsync(Offset + index, length, value);
        }


        public async Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value)
        {
            if (length > Length)
            {
                throw new ArgumentException(Properties.Resources.WriteOnlyBinaryDataAccessor_SourceDataTooLarge);
            }

            await Data.WriteAsync(Offset + index, length, value);
        }
     }

    public static class IWriteOnlyBinaryDataAccessorExtensions
    {
        /// <summary>
        /// Gets a view on top of the current data
        /// </summary>
        /// <param name="data">Data to reference</param>
        /// <param name="offset">Offset of the view</param>
        /// <param name="length">Maximum length of the view</param>
        /// <returns>A view on top of the data</returns>
        public static WriteOnlyBinaryDataAccessorReference GetWriteOnlyDataReference(this IWriteOnlyBinaryDataAccessor data, long offset, long length)
        {
            if (data is WriteOnlyBinaryDataAccessorReference reference)
            {
                return new WriteOnlyBinaryDataAccessorReference(reference, offset, length);
            }
            else
            {
                return new WriteOnlyBinaryDataAccessorReference(data, offset, length);
            }
        }
    }
}
