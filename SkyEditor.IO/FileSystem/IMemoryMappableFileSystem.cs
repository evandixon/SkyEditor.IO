using System.IO.MemoryMappedFiles;

namespace SkyEditor.IO.FileSystem
{
    public interface IMemoryMappableFileSystem : IFileSystem
    {
        MemoryMappedFile OpenMemoryMappedFile(string filename);
    }
}
