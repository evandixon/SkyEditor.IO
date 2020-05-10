using SkyEditor.IO.Binary.Internal;
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
    public interface IReadOnlyBinaryDataAccessor : ISeekable
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
        Task<byte[]> ReadArrayAsync();

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        ReadOnlySpan<byte> ReadSpan();

        /// <summary>
        /// Reads all of the available data
        /// </summary>
        Task<ReadOnlyMemory<byte>> ReadMemoryAsync();

        #region Random Access

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
        #endregion
    }

    public static class IReadOnlyBinaryDataAccessorExtensions
    {
        public static IReadOnlyBinaryDataAccessor Slice(this IReadOnlyBinaryDataAccessor accessor, long offset, long length)
        {
            return accessor switch
            {
                ArrayBinaryDataAccessor arrayAccessor => arrayAccessor.Slice(offset, length),
                MemoryBinaryDataAccessor memoryAccessor => memoryAccessor.Slice(offset, length),
                ReadOnlyBinaryDataAccessorReference reference => new ReadOnlyBinaryDataAccessorReference(reference, offset, length),
                _ => new ReadOnlyBinaryDataAccessorReference(accessor, offset, length)
            };
        }

        #region Integer/Float Reads

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static short ReadInt16(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt16LittleEndian(accessor.ReadSpan(offset, sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<short> ReadInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(short));
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static int ReadInt32(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt32LittleEndian(accessor.ReadSpan(offset, sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<int> ReadInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(int));
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static long ReadInt64(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt64LittleEndian(accessor.ReadSpan(offset, sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<long> ReadInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(long));
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static ushort ReadUInt16(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt16LittleEndian(accessor.ReadSpan(offset, sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<ushort> ReadUInt16Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(ushort));
            return BinaryPrimitives.ReadUInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static uint ReadUInt32(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt32LittleEndian(accessor.ReadSpan(offset, sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<uint> ReadUInt32Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(uint));
            return BinaryPrimitives.ReadUInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static ulong ReadUInt64(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt64LittleEndian(accessor.ReadSpan(offset, sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<ulong> ReadUInt64Async(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(ulong));
            return BinaryPrimitives.ReadUInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a little endian single-precision floating point number
        /// </summary>
        /// <param name="offset">Offset of the float to read</param>
        /// <returns>The float from the given location</returns>
        public static float ReadSingle(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
#if NETSTANDARD2_0
            return BitConverter.ToSingle(accessor.ReadArray(offset, sizeof(float)), 0);
#else
            return BitConverter.ToSingle(accessor.ReadSpan(offset, sizeof(float)));
#endif
        }

        /// <summary>
        /// Reads a little endian single-precision floating point number
        /// </summary>
        /// <param name="offset">Offset of the float to read</param>
        /// <returns>The float from the given location</returns>
        public static async Task<float> ReadSingleAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
#if NETSTANDARD2_0
            return BitConverter.ToSingle(await accessor.ReadArrayAsync(offset, sizeof(float)), 0);
#else
            return BitConverter.ToSingle((await accessor.ReadMemoryAsync(offset, sizeof(float))).Span);
#endif
        }

        /// <summary>
        /// Reads a little endian double-precision floating point number
        /// </summary>
        /// <param name="offset">Offset of the double to read</param>
        /// <returns>The double from the given location</returns>
        public static double ReadDouble(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
#if NETSTANDARD2_0
            return BitConverter.ToDouble(accessor.ReadArray(offset, sizeof(double)), 0);
#else
            return BitConverter.ToDouble(accessor.ReadSpan(offset, sizeof(double)));
#endif
        }

        /// <summary>
        /// Reads a little endian double-precision floating point number
        /// </summary>
        /// <param name="offset">Offset of the double to read</param>
        /// <returns>The double from the given location</returns>
        public static async Task<double> ReadDoubleAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
#if NETSTANDARD2_0
            return BitConverter.ToDouble(await accessor.ReadArrayAsync(offset, sizeof(double)), 0);
#else
            return BitConverter.ToDouble((await accessor.ReadMemoryAsync(offset, sizeof(double))).Span);
#endif
        }

        #endregion

        #region Big Endian Reads

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static short ReadInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt16BigEndian(accessor.ReadSpan(offset, sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<short> ReadInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(short));
            return BinaryPrimitives.ReadInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static int ReadInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt32BigEndian(accessor.ReadSpan(offset, sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<int> ReadInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(int));
            return BinaryPrimitives.ReadInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static long ReadInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadInt64BigEndian(accessor.ReadSpan(offset, sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<long> ReadInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(long));
            return BinaryPrimitives.ReadInt64BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static ushort ReadUInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt16BigEndian(accessor.ReadSpan(offset, sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<ushort> ReadUInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(ushort));
            return BinaryPrimitives.ReadUInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static uint ReadUInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt32BigEndian(accessor.ReadSpan(offset, sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<uint> ReadUInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(uint));
            return BinaryPrimitives.ReadUInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static ulong ReadUInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor, long offset) => BinaryPrimitives.ReadUInt64BigEndian(accessor.ReadSpan(offset, sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        public static async Task<ulong> ReadUInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor, long offset)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(offset, sizeof(ulong));
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
#if NETSTANDARD2_0
            return Encoding.Unicode.GetString(accessor.ReadArray(index, length * 2));
#else
            return Encoding.Unicode.GetString(accessor.ReadSpan(index, length * 2));
#endif
        }

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadUnicodeStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length)
        {
#if NETSTANDARD2_0
            return Encoding.Unicode.GetString(await accessor.ReadArrayAsync(index, length * 2));
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(index, length * 2);
            return Encoding.Unicode.GetString(bytes.Span);
#endif
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
            return await accessor.ReadUnicodeStringAsync(index, length);
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

            return await accessor.ReadStringAsync(index, length, e);
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The string at the given offset</returns>
        public static string ReadString(this IReadOnlyBinaryDataAccessor accessor, long index, int length, Encoding e)
        {
#if NETSTANDARD2_0
            return e.GetString(accessor.ReadArray(index, length));
#else
            return e.GetString(accessor.ReadSpan(index, length));
#endif
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static async Task<string> ReadStringAsync(this IReadOnlyBinaryDataAccessor accessor, long index, int length, Encoding e)
        {
#if NETSTANDARD2_0
            return e.GetString(await accessor.ReadArrayAsync(index, length));
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadMemoryAsync(index, length);
            return e.GetString(bytes.Span);
#endif
        }
        #endregion

        #region Sequential Data Access

        /// <summary>
        /// Reads the next available byte. This method is not thread-safe.
        /// </summary>
        public static byte ReadNextByte(this IReadOnlyBinaryDataAccessor accessor)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return sequentialDataAccessor.ReadNextByte();
            }

            var value = accessor.ReadByte(accessor.Position);
            accessor.Position += sizeof(byte);
            return value;
        }

        /// <summary>
        /// Reads the next available byte. This method is not thread-safe.
        /// </summary>
        public static async Task<byte> ReadNextByteAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return await sequentialDataAccessor.ReadNextByteAsync();
            }
            var value = await accessor.ReadByteAsync(accessor.Position);
            accessor.Position += sizeof(byte);
            return value;
        }

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        public static byte[] ReadNextArray(this IReadOnlyBinaryDataAccessor accessor, int length)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return sequentialDataAccessor.ReadNextArray(length);
            }
            var value = accessor.ReadArray(accessor.Position, length);
            accessor.Position += value.Length;
            return value;
        }

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        public static ReadOnlySpan<byte> ReadNextSpan(this IReadOnlyBinaryDataAccessor accessor, int length)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return sequentialDataAccessor.ReadNextSpan(length);
            }
            var value = accessor.ReadSpan(accessor.Position, length);
            accessor.Position += value.Length;
            return value;
        }

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        public static async Task<byte[]> ReadNextArrayAsync(this IReadOnlyBinaryDataAccessor accessor, int length)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return await sequentialDataAccessor.ReadNextArrayAsync(length);
            }
            var value = await accessor.ReadArrayAsync(accessor.Position, length);
            accessor.Position += value.Length;
            return value;
        }

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to readd</param>
        public static async Task<ReadOnlyMemory<byte>> ReadNextMemoryAsync(this IReadOnlyBinaryDataAccessor accessor, int length)
        {
            if (accessor is IReadOnlyBinarySequentialDataAccessor sequentialDataAccessor)
            {
                return await sequentialDataAccessor.ReadNextMemoryAsync(length);
            }

            var value = await accessor.ReadMemoryAsync(accessor.Position, length);
            accessor.Position += value.Length;
            return value;
        }
        #endregion

        #region Sequential Integer/Float Reads

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        public static short ReadNextInt16(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt16LittleEndian(accessor.ReadNextSpan(sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        public static async Task<short> ReadNextInt16Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(short));
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        public static int ReadNextInt32(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt32LittleEndian(accessor.ReadNextSpan(sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        public static async Task<int> ReadNextInt32Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(int));
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        public static long ReadNextInt64(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt64LittleEndian(accessor.ReadNextSpan(sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        public static async Task<long> ReadNextInt64Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(long));
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        public static ushort ReadNextUInt16(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt16LittleEndian(accessor.ReadNextSpan(sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        public static async Task<ushort> ReadNextUInt16Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(ushort));
            return BinaryPrimitives.ReadUInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        public static uint ReadNextUInt32(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt32LittleEndian(accessor.ReadNextSpan(sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        public static async Task<uint> ReadNextUInt32Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(uint));
            return BinaryPrimitives.ReadUInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        public static ulong ReadNextUInt64(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt64LittleEndian(accessor.ReadNextSpan(sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        public static async Task<ulong> ReadNextUInt64Async(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(ulong));
            return BinaryPrimitives.ReadUInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a little endian single-precision floating point number
        /// </summary>
        /// <param name="offset">Offset of the float to read</param>
        public static float ReadNextSingle(this IReadOnlyBinaryDataAccessor accessor)
        {
#if NETSTANDARD2_0
            return BitConverter.ToSingle(accessor.ReadNextArray(sizeof(float)), 0);
#else
            return BitConverter.ToSingle(accessor.ReadNextSpan(sizeof(float)));
#endif
        }

        /// <summary>
        /// Reads a little endian single-precision floating point number
        /// </summary>
        public static async Task<float> ReadNextSingleAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
#if NETSTANDARD2_0
            return BitConverter.ToSingle(await accessor.ReadNextArrayAsync(sizeof(float)), 0);
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(float));
            return BitConverter.ToSingle(bytes.Span);
#endif
        }

        /// <summary>
        /// Reads a little endian double-precision floating point number
        /// </summary>
        public static double ReadNextDouble(this IReadOnlyBinaryDataAccessor accessor)
        {
#if NETSTANDARD2_0
            return BitConverter.ToDouble(accessor.ReadNextArray(sizeof(double)), 0);
#else
            return BitConverter.ToDouble(accessor.ReadNextSpan(sizeof(double)));
#endif
        }

        /// <summary>
        /// Reads a little endian double-precision floating point number
        /// </summary>
        public static async Task<double> ReadNextDoubleAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
#if NETSTANDARD2_0
            return BitConverter.ToDouble(await accessor.ReadNextArrayAsync(sizeof(double)), 0);
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(double));
            return BitConverter.ToDouble(bytes.Span);
#endif
        }
        #endregion

        #region Sequential Big Endian Integer/Float Reads

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        public static short ReadNextInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt16BigEndian(accessor.ReadNextSpan(sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        public static async Task<short> ReadNextInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(short));
            return BinaryPrimitives.ReadInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        public static int ReadNextInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt32BigEndian(accessor.ReadNextSpan(sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        public static async Task<int> ReadNextInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(int));
            return BinaryPrimitives.ReadInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        public static long ReadNextInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadInt64BigEndian(accessor.ReadNextSpan(sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        public static async Task<long> ReadNextInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(long));
            return BinaryPrimitives.ReadInt64BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        public static ushort ReadNextUInt16BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt16BigEndian(accessor.ReadNextSpan(sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        public static async Task<ushort> ReadNextUInt16BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(ushort));
            return BinaryPrimitives.ReadUInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        public static uint ReadNextUInt32BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt32BigEndian(accessor.ReadNextSpan(sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        public static async Task<uint> ReadNextUInt32BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(uint));
            return BinaryPrimitives.ReadUInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        public static ulong ReadNextUInt64BigEndian(this IReadOnlyBinaryDataAccessor accessor) => BinaryPrimitives.ReadUInt64BigEndian(accessor.ReadNextSpan(sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        public static async Task<ulong> ReadNextUInt64BigEndianAsync(this IReadOnlyBinaryDataAccessor accessor)
        {
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(sizeof(ulong));
            return BinaryPrimitives.ReadUInt64BigEndian(bytes.Span);
        }
        #endregion

        #region Sequential String Reads

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        public static string ReadNextUnicodeString(this IReadOnlyBinaryDataAccessor accessor, int length) 
        {
#if NETSTANDARD2_0
            return Encoding.Unicode.GetString(accessor.ReadNextArray(length * 2));
#else
            return Encoding.Unicode.GetString(accessor.ReadNextSpan(length * 2));
#endif
        }

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The next UTF-16 string</returns>
        public static async Task<string> ReadNextUnicodeStringAsync(this IReadOnlyBinaryDataAccessor accessor, int length)
        {
#if NETSTANDARD2_0
            return Encoding.Unicode.GetString(await accessor.ReadNextArrayAsync(length * 2));
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(length * 2);
            return Encoding.Unicode.GetString(bytes.Span);
#endif
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="length">Length in bytes of the data to read</param>
        /// <returns>The next string</returns>
        public static string ReadNextString(this IReadOnlyBinaryDataAccessor accessor, int length, Encoding e)
        {
#if NETSTANDARD2_0
            return e.GetString(accessor.ReadNextArray(length));
#else
            return e.GetString(accessor.ReadNextSpan(length));
#endif
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The next UTF-16 string</returns>
        public static async Task<string> ReadNextStringAsync(this IReadOnlyBinaryDataAccessor accessor, int length, Encoding e)
        {
#if NETSTANDARD2_0
            return e.GetString(await accessor.ReadNextArrayAsync(length));
#else
            ReadOnlyMemory<byte> bytes = await accessor.ReadNextMemoryAsync(length);
            return e.GetString(bytes.Span);
#endif
        }
        #endregion
    }
}
