using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO
{
    /// <summary>
    /// Provides write access to binary data.
    /// </summary>
    /// <remarks>
    /// Thread safety may vary by implementation
    /// </remarks>
    public interface IWriteOnlyBinaryDataAccessor
    {
        /// <summary>
        /// Replaces the available data with the given value
        /// </summary>
        /// <param name="value">Data to write</param>
        void Write(byte[] value);

        /// <summary>
        /// Replaces the available data with the given value
        /// </summary>
        /// <param name="value">Data to write</param>
        void Write(ReadOnlySpan<byte> value);

        /// <summary>
        /// Replaces the available data with the given value
        /// </summary>
        /// <param name="value">Data to write</param>
        Task WriteAsync(byte[] value);

        /// <summary>
        /// Replaces the available data with the given value
        /// </summary>
        /// <param name="value">Data to write</param>
        Task WriteAsync(ReadOnlyMemory<byte> value);

        /// <summary>
        /// Writes a byte to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">The byte to write</param>
        void Write(long index, byte value);

        /// <summary>
        /// Writes a byte to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">The byte to write</param>
        Task WriteAsync(long index, byte value);

        /// <summary>
        /// Writes the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="length">Upper bound of the data to write</param>
        /// <param name="value">Data to write</param>
        void Write(long index, int length, byte[] value);

        /// <summary>
        /// Writes the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="length">Upper bound of the data to write</param>
        /// <param name="value">Data to write</param>
        void Write(long index, int length, ReadOnlySpan<byte> value);

        /// <summary>
        /// Writes the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="length">Upper bound of the data to write</param>
        /// <param name="value">Data to write</param>
        Task WriteAsync(long index, int length, byte[] value);

        /// <summary>
        /// Writes the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="length">Upper bound of the data to write</param>
        /// <param name="value">Data to write</param>
        Task WriteAsync(long index, int length, ReadOnlyMemory<byte> value);
    }

    public static class IWriteOnlyBinaryDataAccessorExtensions
    {
        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static void Write(this IWriteOnlyBinaryDataAccessor accessor, long index, byte[] value)
        {
            accessor.Write(index, value.Length, value);
        }

        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static async Task WriteAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, byte[] value)
        {
            await accessor.WriteAsync(index, value.Length, value);
        }

        #region Integer Writes
        /// <summary>
        /// Writes a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt16(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int16 value)
        {
            accessor.Write(offset, 2, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt16Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int16 value)
        {
            await accessor.WriteAsync(offset, 2, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt32(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int32 value)
        {
            accessor.Write(offset, 4, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt32Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int32 value)
        {
            await accessor.WriteAsync(offset, 4, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt64(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int64 value)
        {
            accessor.Write(offset, 8, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt64Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, Int64 value)
        {
            await accessor.WriteAsync(offset, 8, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt16(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt16 value)
        {
            accessor.Write(offset, 2, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt16Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt16 value)
        {
            await accessor.WriteAsync(offset, 2, BitConverter.GetBytes(value));
        }


        /// <summary>
        /// Writes an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt32(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt32 value)
        {
            accessor.Write(offset, 4, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt32Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt32 value)
        {
            await accessor.WriteAsync(offset, 4, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt64(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt64 value)
        {
            accessor.Write(offset, 8, BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to write.</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt64Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, UInt64 value)
        {
            await accessor.WriteAsync(offset, 8, BitConverter.GetBytes(value));
        }
        #endregion

        #region String Writes

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written without an ending null character.</param>
        public static void WriteString(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value)
        {
            accessor.Write(index, e.GetBytes(value));
        }

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written without an ending null character.</param>
        public static async Task WriteStringAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value)
        {
            await accessor.WriteAsync(index, e.GetBytes(value));
        }

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written with an ending null character.</param>
        public static void WriteNullTerminatedString(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value)
        {
            var nullChar = e.GetBytes(new[] { Convert.ToChar(0) });
            var data = e.GetBytes(value);
            accessor.Write(index, data.Length, data);
            accessor.Write(index + data.Length, nullChar.Length, nullChar);
        }

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written with an ending null character.</param>
        public static async Task WriteNullTerminatedStringAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value)
        {
            var nullChar = e.GetBytes(new[] { Convert.ToChar(0) });
            var data = e.GetBytes(value);
            await accessor.WriteAsync(index, data.Length, data);
            await accessor.WriteAsync(index + data.Length, nullChar.Length, nullChar);
        }

        #endregion
    }
}
