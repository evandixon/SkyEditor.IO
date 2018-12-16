using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO
{
    /// <summary>
    /// Provides read access to binary data
    /// </summary>
    /// <remarks>
    /// Thread safety may vary by implementation
    /// </remarks>
    public interface IReadOnlyBinaryDataAccessor
    {
        /// <summary>
        /// Length of the data, in bytes
        /// </summary>
        long Length { get; }

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        byte[] ReadArray();

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        ReadOnlySpan<byte> ReadSpan();

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        Task<byte[]> ReadArrayAsync();

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        Task<ReadOnlyMemory<byte>> ReadMemoryAsync();

        /// <summary>
        /// Reads a byte at the given index
        /// </summary>
        /// <param name="index">Index of the byte</param>
        byte ReadByte(long index);

        /// <summary>
        /// Reads a byte at the given index
        /// </summary>
        /// <param name="index">Index of the byte</param>
        Task<byte> ReadByteAsync(long index);

        /// <summary>
        /// Reads a subset of the available data
        /// </summary>
        /// <param name="index">Index of the desired data</param>
        /// <param name="length"></param>
        byte[] ReadArray(long index, int length);

        /// <summary>
        /// Reads a subset of the available data
        /// </summary>
        /// <param name="index">Index of the desired data</param>
        /// <param name="length"></param>
        ReadOnlySpan<byte> ReadSpan(long index, int length);

        /// <summary>
        /// Reads a subset of the available data
        /// </summary>
        /// <param name="index">Index of the desired data</param>
        /// <param name="length">Length of data to read</param>
        Task<byte[]> ReadArrayAsync(long index, int length);

        /// <summary>
        /// Reads a subset of the available data
        /// </summary>
        /// <param name="index">Index of the desired data</param>
        /// <param name="length">Length of data to read</param>
        Task<ReadOnlyMemory<byte>> ReadMemoryAsync(long index, int length);
    }

    public static class IReadOnlyBinaryDataAccessorExtensions
    {
        #region Integer Reads

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int16 ReadInt16(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt16(accessor.ReadSpan(offset, 2));
        }

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int16> ReadInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt16(await accessor.ReadArrayAsync(offset, 2), 0);
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int32 ReadInt32(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt32(accessor.ReadSpan(offset, 4));
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int32> ReadInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt32(await accessor.ReadArrayAsync(offset, 4), 0);
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int64 ReadInt64(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt64(accessor.ReadSpan(offset, 8));
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int64> ReadInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToInt64(await accessor.ReadArrayAsync(offset, 8), 0);
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt16 ReadUInt16(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt16(accessor.ReadSpan(offset, 2));
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt16> ReadUInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt16(await accessor.ReadArrayAsync(offset, 2), 0);
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt32 ReadUInt32(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt32(accessor.ReadSpan(offset, 4));
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt32> ReadUInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt32(await accessor.ReadArrayAsync(offset, 4), 0);
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt64 ReadUInt64(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt64(accessor.ReadSpan(offset, 8));
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt64> ReadUInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BitConverter.ToUInt64(await accessor.ReadArrayAsync(offset, 8), 0);
        }
        #endregion

        #region Big Endian Reads

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int16 ReadInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 2);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int16> ReadInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 2);
            Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int32 ReadInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int32> ReadInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 4);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int64 ReadInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 8);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int64> ReadInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 8);
            Array.Reverse(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt16 ReadUInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 2);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt16> ReadUInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 2);
            Array.Reverse(bytes);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt32 ReadUInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt32> ReadUInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 4);
            Array.Reverse(bytes);
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt64 ReadUInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = accessor.ReadArray(offset, 8);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt64> ReadUInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            var bytes = await accessor.ReadArrayAsync(offset, 8);
            Array.Reverse(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }
        #endregion

        #region String Reads

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static string ReadUnicodeString(this IReadOnlyBinaryDataAccessor accessor, long index, int length)
        {
            return Encoding.Unicode.GetString(accessor.ReadArray(index, length * 2), 0, length * 2);
        }

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadUnicodeStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length)
        {
            return Encoding.Unicode.GetString(await accessor.ReadArrayAsync(index, length * 2), 0, length * 2);
        }

        /// <summary>
        /// Reads a null-terminated UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The UTF-16 string</returns>
        public static string ReadNullTerminatedUnicodeString(this IReadOnlyBinaryDataAccessor accessor, long index)
        {
            int length = 0;
            while (accessor.ReadByte(index + length * 2) != 0 || accessor.ReadByte(index + length * 2 + 1) != 0)
            {
                length += 1;
            }
            return accessor.ReadUnicodeString(index, length);
        }

        /// <summary>
        /// Reads a null-terminated UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The UTF-16 string</returns>
        public static async Task<string> ReadNullTerminatedUnicodeStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index)
        {
            int length = 0;
            while (await accessor.ReadByteAsync(index + length * 2) != 0 || await accessor.ReadByteAsync(index + length * 2 + 1) != 0)
            {
                length += 1;
            }
            return accessor.ReadUnicodeString(index, length);
        }

        /// <summary>
        /// Reads a null-terminated string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The string at the given location</returns>
        public static string ReadNullTerminatedString(this IReadOnlyBinaryDataAccessor accessor, long index, Encoding e)
        {
            // The null character we're looking for
            var nullCharSequence = e.GetBytes(Convert.ToChar(0x0).ToString());

            // Find the length of the string as determined by the location of the null-char sequence
            int length = 0;
            while (!accessor.ReadArray(index + length * nullCharSequence.Length, nullCharSequence.Length).All(x => x == 0))
            {
                length += 1;
            }

            return accessor.ReadString(index, length, e);
        }

        /// <summary>
        /// Reads a null-terminated using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The string at the given location</returns>
        public static async Task<string> ReadNullTerminatedStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, Encoding e)
        {
            // The null character we're looking for
            var nullCharSequence = e.GetBytes(Convert.ToChar(0x0).ToString());

            // Find the length of the string as determined by the location of the null-char sequence
            int length = 0;
            while (!(await accessor.ReadArrayAsync(index + length * nullCharSequence.Length, nullCharSequence.Length)).All(x => x == 0))
            {
                length += 1;
            }

            return accessor.ReadString(index, length, e);
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static string ReadString(this IReadOnlyBinaryDataAccessor accessor, long index, int length, Encoding e)
        {
            return e.GetString(accessor.ReadArray(index, length), 0, length);
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length, Encoding e)
        {
            return e.GetString(await accessor.ReadArrayAsync(index, length), 0, length);
        }
        #endregion
    }
}
