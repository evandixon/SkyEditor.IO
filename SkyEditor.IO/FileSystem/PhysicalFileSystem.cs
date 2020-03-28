using System;
using System.IO;
using System.IO.MemoryMappedFiles;

namespace SkyEditor.IO.FileSystem
{
    /// <summary>
    /// Implementation of <see cref="IFileSystem"/> that wraps <see cref="System.IO.File"/> and <see cref="System.IO.Directory"/> (i.e. the physical file system).
    /// </summary>
    public class PhysicalFileSystem : IFileSystem, IMemoryMappableFileSystem
    {
        private static readonly Lazy<PhysicalFileSystem> InstanceLazy = new Lazy<PhysicalFileSystem>(() => new PhysicalFileSystem());

        public static PhysicalFileSystem Instance => InstanceLazy.Value;

        public PhysicalFileSystem()
        {
            _workingDirectory = Directory.GetCurrentDirectory();
        }

        public string WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                if (Path.IsPathRooted(value))
                {
                    _workingDirectory = value;
                }
                else
                {
                    foreach (var part in value.Replace('\\', '/').Split('/'))
                    {
                        if (part == ".")
                        {
                            // Do nothing
                        }
                        else if (part == "..")
                        {
                            _workingDirectory = Path.GetDirectoryName(_workingDirectory) ?? throw new DirectoryNotFoundException();
                        }
                        else
                        {
                            _workingDirectory = Path.Combine(_workingDirectory, part);
                        }                        
                    }
                }
            }
        }
        private string _workingDirectory;

        protected string FixPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.Combine(WorkingDirectory, path);
            }
        }

        public void ResetWorkingDirectory()
        {
            WorkingDirectory = Directory.GetCurrentDirectory();
        }

        public virtual void CopyFile(string sourceFilename, string destinationFilename)
        {
            File.Copy(FixPath(sourceFilename), FixPath(destinationFilename), true);
        }

        public virtual void CreateDirectory(string path)
        {
            Directory.CreateDirectory(FixPath(path));
        }

        public virtual void DeleteDirectory(string path)
        {
            Directory.Delete(FixPath(path), true);
        }

        public virtual void DeleteFile(string filename)
        {
            File.Delete(FixPath(filename));
        }

        public virtual void WriteAllBytes(string filename, byte[] data)
        {
            File.WriteAllBytes(FixPath(filename), data);
        }

        public virtual void WriteAllText(string filename, string data)
        {
            File.WriteAllText(FixPath(filename), data);
        }

        public virtual bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(FixPath(directoryPath));
        }

        public virtual bool FileExists(string Filename)
        {
            return File.Exists(FixPath(Filename));
        }

        public virtual string[] GetDirectories(string path, bool topDirectoryOnly)
        {
            if (topDirectoryOnly)
            {
                return Directory.GetDirectories(FixPath(path), "*", SearchOption.TopDirectoryOnly);
            }
            else
            {
                return Directory.GetDirectories(FixPath(path), "*", SearchOption.AllDirectories);
            }
        }

        public virtual long GetFileLength(string filename)
        {
            return (new FileInfo(FixPath(filename))).Length;
        }

        public virtual string[] GetFiles(string path, string searchPattern, bool topDirectoryOnly)
        {
            if (topDirectoryOnly)
            {
                return Directory.GetFiles(FixPath(path), searchPattern, SearchOption.TopDirectoryOnly);
            }
            else
            {
                return Directory.GetFiles(FixPath(path), searchPattern, SearchOption.AllDirectories);
            }
        }

        public virtual string GetTempDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "SkyEditor", Guid.NewGuid().ToString());
            if (!DirectoryExists(tempDir))
            {
                CreateDirectory(tempDir);
            }
            return tempDir;
        }

        public virtual string GetTempFilename()
        {
            return Path.GetTempFileName();
        }

        public virtual Stream OpenFile(string filename)
        {
            return File.Open(FixPath(filename), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        }

        public virtual Stream OpenFileReadOnly(string filename)
        {
            return File.Open(FixPath(filename), FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
        }

        public virtual Stream OpenFileWriteOnly(string filename)
        {
            return File.Open(FixPath(filename), FileMode.OpenOrCreate, FileAccess.Write);
        }

        public virtual byte[] ReadAllBytes(string filename)
        {
            return File.ReadAllBytes(FixPath(filename));
        }

        public virtual string ReadAllText(string filename)
        {
            return File.ReadAllText(FixPath(filename));
        }

        public MemoryMappedFile OpenMemoryMappedFile(string filename)
        {
            return MemoryMappedFile.CreateFromFile(filename);
        }
    }
}
