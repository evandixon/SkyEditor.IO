using System;

namespace SkyEditor.IO.Binary
{
    public interface IBinaryDataAccessor : IReadOnlyBinaryDataAccessor, IWriteOnlyBinaryDataAccessor
    {
        new public IBinaryDataAccessor Slice(long offset, long length)
        {
            return this switch
            {
                BinaryDataAccessorReference reference => new BinaryDataAccessorReference(reference, offset, length),
                _ => new BinaryDataAccessorReference(this, offset, length),
            };
        }
    }

    public static class IBinaryDataAccessorExtensions 
    {
        public static IBinaryDataAccessor Slice(this IBinaryDataAccessor accessor, long offset, long length) => accessor.Slice(offset, length);
    }
}
