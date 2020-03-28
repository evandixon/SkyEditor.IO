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

#region Integer Reads

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        short ReadInt16(long offset) => BinaryPrimitives.ReadInt16LittleEndian(ReadSpan(offset, sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<short> ReadInt16Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(short));
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        int ReadInt32(long offset) => BinaryPrimitives.ReadInt32LittleEndian(ReadSpan(offset, sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<int> ReadInt32Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(int));
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        long ReadInt64(long offset) => BinaryPrimitives.ReadInt64LittleEndian(ReadSpan(offset, sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<long> ReadInt64Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(long));
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        ushort ReadUInt16(long offset) => BinaryPrimitives.ReadUInt16LittleEndian(ReadSpan(offset, sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<ushort> ReadUInt16Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(ushort));
            return BinaryPrimitives.ReadUInt16LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        uint ReadUInt32(long offset) => BinaryPrimitives.ReadUInt32LittleEndian(ReadSpan(offset, sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<uint> ReadUInt32Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(uint));
            return BinaryPrimitives.ReadUInt32LittleEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        ulong ReadUInt64(long offset) => BinaryPrimitives.ReadUInt64LittleEndian(ReadSpan(offset, sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<ulong> ReadUInt64Async(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(ulong));
            return BinaryPrimitives.ReadUInt64LittleEndian(bytes.Span);
        }
#endregion

#region Big Endian Reads

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        short ReadInt16BigEndian(long offset) => BinaryPrimitives.ReadInt16BigEndian(ReadSpan(offset, sizeof(short)));

        /// <summary>
        /// Reads a signed 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<short> ReadInt16BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(short));
            return BinaryPrimitives.ReadInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        int ReadInt32BigEndian(long offset) => BinaryPrimitives.ReadInt32BigEndian(ReadSpan(offset, sizeof(int)));

        /// <summary>
        /// Reads a signed 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<int> ReadInt32BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(int));
            return BinaryPrimitives.ReadInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        long ReadInt64BigEndian(long offset) => BinaryPrimitives.ReadInt64BigEndian(ReadSpan(offset, sizeof(long)));

        /// <summary>
        /// Reads a signed 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<long> ReadInt64BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(long));
            return BinaryPrimitives.ReadInt64BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        ushort ReadUInt16BigEndian(long offset) => BinaryPrimitives.ReadUInt16BigEndian(ReadSpan(offset, sizeof(ushort)));

        /// <summary>
        /// Reads an unsigned 16 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<ushort> ReadUInt16BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(ushort));
            return BinaryPrimitives.ReadUInt16BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        uint ReadUInt32BigEndian(long offset) => BinaryPrimitives.ReadUInt32BigEndian(ReadSpan(offset,sizeof(uint)));

        /// <summary>
        /// Reads an unsigned 32 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<uint> ReadUInt32BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(uint));
            return BinaryPrimitives.ReadUInt32BigEndian(bytes.Span);
        }

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        ulong ReadUInt64BigEndian(long offset) => BinaryPrimitives.ReadUInt64BigEndian(ReadSpan(offset, sizeof(ulong)));

        /// <summary>
        /// Reads an unsigned 64 bit big endian integer
        /// </summary>
        /// <param name="offset">Offset of the integer to read.</param>
        /// <returns>The integer from the given location</returns>
        async Task<ulong> ReadUInt64BigEndianAsync(long offset)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(offset, sizeof(ulong));
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
        string ReadUnicodeString(long index, int length) => Encoding.Unicode.GetString(ReadSpan(index, length * 2));

        /// <summary>
        /// Reads a UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        async Task<string> ReadUnicodeStringAsync(long index, int length)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(index, length * 2);
            return Encoding.Unicode.GetString(bytes.Span);
        }

        /// <summary>
        /// Reads a null-terminated UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The UTF-16 string</returns>
        string ReadNullTerminatedUnicodeString(long index)
        {
            int length = 0;
            while (ReadByte(index + length * 2) != 0 || ReadByte(index + length * 2 + 1) != 0)
            {
                length += 1;
            }
            return ReadUnicodeString(index, length);
        }

        /// <summary>
        /// Reads a null-terminated UTF-16 string
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The UTF-16 string</returns>
        async Task<string> ReadNullTerminatedUnicodeStringAsync(long index)
        {
            int length = 0;
            while (await ReadByteAsync(index + length * 2) != 0 || await ReadByteAsync(index + length * 2 + 1) != 0)
            {
                length += 1;
            }
            return ReadUnicodeString(index, length);
        }

        /// <summary>
        /// Reads a null-terminated string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The string at the given location</returns>
        string ReadNullTerminatedString(long index, Encoding e)
        {
            // The null character we're looking for
            var nullCharSequence = e.GetBytes("\0");

            // Find the length of the string as determined by the location of the null-char sequence
            int length = 0;
            while (!ReadArray(index + length * nullCharSequence.Length, nullCharSequence.Length).All(x => x == 0))
            {
                length += 1;
            }

            return ReadString(index, length, e);
        }

        /// <summary>
        /// Reads a null-terminated using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <returns>The string at the given location</returns>
        async Task<string> ReadNullTerminatedStringAsync(long index, Encoding e)
        {
            // The null character we're looking for
            var nullCharSequence = e.GetBytes(Convert.ToChar(0x0).ToString());

            // Find the length of the string as determined by the location of the null-char sequence
            int length = 0;
            while (!(await ReadArrayAsync(index + length * nullCharSequence.Length, nullCharSequence.Length)).All(x => x == 0))
            {
                length += 1;
            }

            return ReadString(index, length, e);
        }

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        string ReadString(long index, int length, Encoding e) => e.GetString(ReadSpan(index, length));

        /// <summary>
        /// Reads a string using the given encoding
        /// </summary>
        /// <param name="offset">Offset of the string</param>
        /// <param name="length">Length in characters of the string</param>
        /// <returns>The UTF-16 string at the given offset</returns>
        async Task<string> ReadStringAsync(long index, int length, Encoding e)
        {
            ReadOnlyMemory<byte> bytes = await ReadMemoryAsync(index, length);
            return e.GetString(bytes.Span);
        }
#endregion
    }
}
