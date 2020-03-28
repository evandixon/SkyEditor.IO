using System;
using System.Buffers.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
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

        IReadOnlyBinaryDataAccessor Slice(long offset, long length)
        {
            return this switch
            {
                ReadOnlyBinaryDataAccessorReference reference => new ReadOnlyBinaryDataAccessorReference(reference, offset, length),
                _ => new ReadOnlyBinaryDataAccessorReference(this, offset, length)
            };
        }
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
            return BinaryPrimitives.ReadInt16LittleEndian(accessor.ReadSpan(offset, sizeof(Int16)));
        }

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int16> ReadInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int16));
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int32 ReadInt32(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadInt32LittleEndian(accessor.ReadSpan(offset, sizeof(Int32)));
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int32> ReadInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int32));
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int64 ReadInt64(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadInt64LittleEndian(accessor.ReadSpan(offset, sizeof(Int64)));
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int64> ReadInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int64));
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt16 ReadUInt16(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt16LittleEndian(accessor.ReadSpan(offset, sizeof(UInt16)));
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt16> ReadUInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt16));
            return BinaryPrimitives.ReadUInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt32 ReadUInt32(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt32LittleEndian(accessor.ReadSpan(offset, sizeof(UInt32)));
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt32> ReadUInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt32));
            return BinaryPrimitives.ReadUInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt64 ReadUInt64(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt64LittleEndian(accessor.ReadSpan(offset, sizeof(UInt64)));
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt64> ReadUInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt64));
            return BinaryPrimitives.ReadUInt64LittleEndian(bytes.Span);
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
            return BinaryPrimitives.ReadInt16BigEndian(accessor.ReadSpan(offset, sizeof(Int16)));
        }

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int16> ReadInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int16));
            return BinaryPrimitives.ReadInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int32 ReadInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadInt32BigEndian(accessor.ReadSpan(offset, sizeof(Int32)));
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int32> ReadInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int32));
            return BinaryPrimitives.ReadInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static Int64 ReadInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadInt64BigEndian(accessor.ReadSpan(offset, sizeof(Int64)));
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<Int64> ReadInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(Int64));
            return BinaryPrimitives.ReadInt64BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt16 ReadUInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt16BigEndian(accessor.ReadSpan(offset, sizeof(UInt16)));
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt16> ReadUInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt16));
            return BinaryPrimitives.ReadUInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt32 ReadUInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt32BigEndian(accessor.ReadSpan(offset,sizeof(UInt32)));
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt32> ReadUInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt32));
            return BinaryPrimitives.ReadUInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static UInt64 ReadUInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            return BinaryPrimitives.ReadUInt64BigEndian(accessor.ReadSpan(offset, sizeof(UInt64)));
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<UInt64> ReadUInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(UInt64));
            return BinaryPrimitives.ReadUInt64BigEndian(bytes.Span);
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
            return Encoding.Unicode.GetString(accessor.ReadSpan(index, length * 2));
        }

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadUnicodeStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(index, length * 2);
            return Encoding.Unicode.GetString(bytes.Span);
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
            var nullCharSequence = e.GetBytes("\0");

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
            return e.GetString(accessor.ReadSpan(index, length));
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length, Encoding e)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(index, length);
            return e.GetString(bytes.Span);
        }
#endregion
    }
}
