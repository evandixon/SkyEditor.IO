using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.IO
{
    public interface IBinaryDataAccessor : IReadOnlyBinaryDataAccessor, IWriteOnlyBinaryDataAccessor
    {
    }
}
