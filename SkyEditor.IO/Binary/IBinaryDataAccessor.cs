using SkyEditor.IO.Binary.Internal;
using System;

namespace SkyEditor.IO.Binary
{
    public interface IBinaryDataAccessor : IReadOnlyBinaryDataAccessor, IWriteOnlyBinaryDataAccessor
    {
    }

    public static class IBinaryDataAccessorExtensions 
    {
        public static IBinaryDataAccessor Slice(this IBinaryDataAccessor accessor, long offset, long length)
        {
            return accessor switch
            {
                ArrayBinaryDataAccessor arrayAccessor => arrayAccessor.Slice(offset, length),
                MemoryBinaryDataAccessor memoryAccessor => memoryAccessor.Slice(offset, length),
                BinaryDataAccessorReference reference => new BinaryDataAccessorReference(reference, offset, length),
                _ => new BinaryDataAccessorReference(accessor, offset, length),
            };
        }
    }
}
