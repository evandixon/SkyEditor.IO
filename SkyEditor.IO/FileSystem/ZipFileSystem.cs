using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SkyEditor.IO.FileSystem
{
    public class ZipFileSystem : IFileSystem
    {
        public ZipFileSystem(ZipArchive zipArchive)
        {
            this.zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
            _workingDirectory = "/";
        }

        private readonly ZipArchive zipArchive;

        public string WorkingDirectory
        {
            get
            {
                return _workingDirectory;
            }
            set
            {
                _workingDirectory = FixDirectory(value);
            }
        }
        private string _workingDirectory;

        private ZipArchiveEntry? GetEntry(string filename)
        {
            return zipArchive.Entries.FirstOrDefault(entry => entry.FullName == filename);
        }

        private string FixDirectory(string directory) => WorkingDirectory + directory.Replace(@"\", "/").TrimStart('/').TrimEnd('/') + "/";

        public long GetFileLength(string filename)
        {
            var file = GetEntry(filename) ?? throw new FileNotFoundException(Properties.Resources.FileSystem_ZipFileSystem_EntryNotFound, filename);
            return file.Length;
        }

        private IEnumerable<string> EnumerateFiles(string path, string searchPattern, bool topDirectoryOnly)
        {
            path = FixDirectory(path);
            var searchPatternRegex = new Regex(InMemoryFileSystem.GetFileSearchRegex(searchPattern));
            var filesInPath = zipArchive.Entries.Where(e => e.FullName.ToLowerInvariant().StartsWith(path));
            if (topDirectoryOnly)
            {
                var slashCount = path.Count(x => x == '/');
                filesInPath = zipArchive.Entries.Where(e => e.FullName.Count(y => y == '/') == slashCount);
            }
            return filesInPath.Where(e => searchPatternRegex.IsMatch(e.Name)).Select(e => e.FullName);
        }

        public string[] GetFiles(string path, string searchPattern, bool topDirectoryOnly)
        {
            return EnumerateFiles(path, searchPattern, topDirectoryOnly).ToArray();
        }

        private IEnumerable<string> EnumerateDirectories(string path, bool topDirectoryOnly)
        {
            var files = EnumerateFiles(path, "*", topDirectoryOnly);
            return files.Select(f => FixDirectory(Path.GetDirectoryName(f))).Distinct();
        }

        public string[] GetDirectories(string path, bool topDirectoryOnly)
        {
            return EnumerateDirectories(path, topDirectoryOnly).ToArray();
        }

        public byte[] ReadAllBytes(string filename)
        {
            var file = GetEntry(filename) ?? throw new FileNotFoundException(Properties.Resources.FileSystem_ZipFileSystem_EntryNotFound, filename);
            if (file.Length > int.MaxValue)
            {
                throw new ArgumentException(Properties.Resources.FileSystem_ZipFileSystem_ReadAllBytes_FileTooLarge, nameof(filename));
            }

            using var stream = file.Open();
            var buffer = new byte[file.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        public Stream OpenFileReadOnly(string filename)
        {
            var file = GetEntry(filename) ?? throw new FileNotFoundException(Properties.Resources.FileSystem_ZipFileSystem_EntryNotFound, filename);
            return new ReadOnlyStream(file.Open());
        }

        public void ResetWorkingDirectory()
        {
            WorkingDirectory = "/";
        }

        public bool FileExists(string filename)
        {
            return GetEntry(filename) != null;
        }

        public bool DirectoryExists(string path)
        {
            var directory = FixDirectory(path);
            return EnumerateDirectories(Path.GetDirectoryName(directory), true).Any(d => d == directory);
        }

        public void CreateDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public void WriteAllBytes(string filename, byte[] data)
        {
            throw new NotImplementedException();
        }

        public void WriteAllText(string filename, string data)
        {
            throw new NotImplementedException();
        }

        public void CopyFile(string sourceFilename, string destinationFilename)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string filename)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public string GetTempFilename()
        {
            throw new NotImplementedException();
        }

        public string GetTempDirectory()
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string filename)
        {
            throw new NotImplementedException();
        }

        public Stream OpenFileWriteOnly(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
