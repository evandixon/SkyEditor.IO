using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.Binary
{
    /// <summary>
    /// A <see cref="IReadOnlyBinaryDataAccessor"/> with a custom implementation for sequential data access
    /// </summary>
    public interface IReadOnlyBinarySequentialDataAccessor
    {
        /// <summary>
        /// Reads the next available byte. This method is not thread-safe.
        /// </summary>
        byte ReadNextByte();

        /// <summary>
        /// Reads the next available byte. This method is not thread-safe.
        /// </summary>
        Task<byte> ReadNextByteAsync();

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        byte[] ReadNextArray(int length);

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        ReadOnlySpan<byte> ReadNextSpan(int length);

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        Task<byte[]> ReadNextArrayAsync(int length);

        /// <summary>
        /// Reads the next series of bytes. This method is not thread-safe.
        /// </summary>
        /// <param name="length">Number of bytes to readd</param>
        Task<ReadOnlyMemory<byte>> ReadNextMemoryAsync(int length);
    }
}
