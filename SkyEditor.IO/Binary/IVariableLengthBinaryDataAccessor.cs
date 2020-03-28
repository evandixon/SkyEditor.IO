using System;

namespace SkyEditor.IO.Binary
{
    public interface IVariableLengthBinaryDataAccessor : IBinaryDataAccessor
    {
        /// <summary>
        /// Sets the length of the underlying data source.
        /// </summary>
        /// <remarks>
        /// Thread safety may vary by implementation, but it is highly probable that this is NOT thread safe.
        /// </remarks>
        /// <exception cref="NotSupportedException">Thrown when the underlying data source does not support a length of the given size.</exception>
        void SetLength(long length);
    }
}
