using SkyEditor.IO.Binary.Internal;
using System;
using System.Buffers.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
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

    // Compatibility layer to allow the use of the interface's default implementation on all classes, without manually casting to the interface
    public static class IWriteOnlyBinaryDataAccessorExtensions
    {
        public static IWriteOnlyBinaryDataAccessor Slice(this IWriteOnlyBinaryDataAccessor accessor, long offset, long length)
        {
            return accessor switch
            {
                ArrayBinaryDataAccessor arrayAccessor => arrayAccessor.Slice(offset, length),
                MemoryBinaryDataAccessor memoryAccessor => memoryAccessor.Slice(offset, length),
                WriteOnlyBinaryDataAccessorReference reference => new WriteOnlyBinaryDataAccessorReference(reference, offset, length),
                _ => new WriteOnlyBinaryDataAccessorReference(accessor, offset, length)
            };
        }

        #region Binary Writes

        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static void Write(this IWriteOnlyBinaryDataAccessor accessor, long index, byte[] value) => accessor.Write(index, value.Length, value);

        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static Task WriteAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, byte[] value) => accessor.WriteAsync(index, value.Length, value);

        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static void Write(this IWriteOnlyBinaryDataAccessor accessor, long index, ReadOnlySpan<byte> value) => accessor.Write(index, value.Length, value);

        /// <summary>
        /// Writes all of the given data to the desired index
        /// </summary>
        /// <param name="index">Index of the data to write</param>
        /// <param name="value">Data to write</param>
        public static Task WriteAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, ReadOnlyMemory<byte> value) => accessor.WriteAsync(index, value.Length, value);

        #endregion

        #region Integer Writes
        /// <summary>
        /// Writes a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt16(this IWriteOnlyBinaryDataAccessor accessor, long offset, short value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(short)];
            BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(short), bytes);
        }

        /// <summary>
        /// Writes a signed 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt16Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, short value)
        {
            Memory<byte> bytes = new byte[sizeof(short)];
            BinaryPrimitives.WriteInt16LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(short), bytes);
        }

        /// <summary>
        /// Writes a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt32(this IWriteOnlyBinaryDataAccessor accessor, long offset, int value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(int)];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(int), bytes);
        }

        /// <summary>
        /// Writes a signed 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt32Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, int value)
        {
            Memory<byte> bytes = new byte[sizeof(int)];
            BinaryPrimitives.WriteInt32LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(int), bytes);
        }

        /// <summary>
        /// Writes a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteInt64(this IWriteOnlyBinaryDataAccessor accessor, long offset, long value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(long)];
            BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(long), bytes);
        }

        /// <summary>
        /// Writes a signed 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteInt64Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, long value)
        {
            Memory<byte> bytes = new byte[sizeof(long)];
            BinaryPrimitives.WriteInt64LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(long), bytes);
        }

        /// <summary>
        /// Writes an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt16(this IWriteOnlyBinaryDataAccessor accessor, long offset, ushort value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ushort)];
            BinaryPrimitives.WriteUInt16LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(ushort), bytes);
        }

        /// <summary>
        /// Writes an unsigned 16 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt16Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, ushort value)
        {
            Memory<byte> bytes = new byte[sizeof(ushort)];
            BinaryPrimitives.WriteInt64LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(ushort), bytes);
        }

        /// <summary>
        /// Writes an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt32(this IWriteOnlyBinaryDataAccessor accessor, long offset, uint value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(uint)];
            BinaryPrimitives.WriteUInt32LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(uint), bytes);
        }

        /// <summary>
        /// Writes an unsigned 32 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt32Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, uint value)
        {
            Memory<byte> bytes = new byte[sizeof(uint)];
            BinaryPrimitives.WriteInt64LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(uint), bytes);
        }

        /// <summary>
        /// Writes an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static void WriteUInt64(this IWriteOnlyBinaryDataAccessor accessor, long offset, ulong value)
        {
            Span<byte> bytes = stackalloc byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64LittleEndian(bytes, value);
            accessor.Write(offset, sizeof(ulong), bytes);
        }

        /// <summary>
        /// Writes an unsigned 64 bit little endian integer
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The integer to write</param>
        public static async Task WriteUInt64Async(this IWriteOnlyBinaryDataAccessor accessor, long offset, ulong value)
        {
            Memory<byte> bytes = new byte[sizeof(ulong)];
            BinaryPrimitives.WriteUInt64LittleEndian(bytes.Span, value);
            await accessor.WriteAsync(offset, sizeof(ulong), bytes);
        }

        /// <summary>
        /// Writes a little endian single-precision floating point number
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The number to write</param>
        public static void WriteSingle(this IWriteOnlyBinaryDataAccessor accessor, long offset, float value)
        {
            var data = BitConverter.GetBytes(value);
            accessor.Write(offset, data);
        }

        /// <summary>
        /// Writes a little endian single-precision floating point number
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The number to write</param>
        public static async Task WriteSingleAsync(this IWriteOnlyBinaryDataAccessor accessor, long offset, float value)
        {
            var data = BitConverter.GetBytes(value);
            await accessor.WriteAsync(offset, data);
        }

        /// <summary>
        /// Writes a little endian double-precision floating point number
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The number to write</param>
        public static void WriteDouble(this IWriteOnlyBinaryDataAccessor accessor, long offset, double value)
        {
            var data = BitConverter.GetBytes(value);
            accessor.Write(offset, data);
        }

        /// <summary>
        /// Writes a little endian double-precision floating point number
        /// </summary>
        /// <param name="offset">Offset to which to write the binary data</param>
        /// <param name="value">The number to write</param>
        public static async Task WriteDoubleAsync(this IWriteOnlyBinaryDataAccessor accessor, long offset, double value)
        {
            var data = BitConverter.GetBytes(value);
            await accessor.WriteAsync(offset, data);
        }
        #endregion

        #region String Writes

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written without an ending null character.</param>
        public static void WriteString(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value) => accessor.Write(index, e.GetBytes(value));

        /// <summary>
        /// Writes a string with the given encoding to the given offset of the file
        /// </summary>
        /// <param name="index">Index of the file to write</param>
        /// <param name="e">The encoding to use</param>
        /// <param name="value">The string to write.  The entire string will be written without an ending null character.</param>
        public static async Task WriteStringAsync(this IWriteOnlyBinaryDataAccessor accessor, long index, Encoding e, string value) => await accessor.WriteAsync(index, e.GetBytes(value));

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
