using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace SkyEditor.IO.FileSystem
{
    public interface IMemoryMappableFileSystem : IFileSystem
    {
        MemoryMappedFile OpenMemoryMappedFile(string filename);
    }
}
