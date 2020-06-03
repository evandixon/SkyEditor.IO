using SkyEditor.IO.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;

namespace SkyEditor.IO.FileSystem
{
    public class ZipFileSystem : IReadOnlyFileSystem
    {
        public ZipFileSystem(ZipArchive zipArchive)
        {
            this.zipArchive = zipArchive ?? throw new ArgumentNullException(nameof(zipArchive));
            _workingDirectory = "/";
        }

        private readonly ZipArchive zipArchive;

        public string WorkingDirectory
        {
            get => _workingDirectory;
            set => _workingDirectory = FixDirectory(value);
        }
        private string _workingDirectory;

        private ZipArchiveEntry? GetEntry(string filename)
        {
            return zipArchive.Entries.FirstOrDefault(entry => entry.FullName == filename);
        }

        private string FixDirectory(string directory)
        {
            var result = (WorkingDirectory + directory.Replace(@"\", "/")).TrimStart('/').TrimEnd('/') + "/";
            if (result == "/")
            {
                return "";
            }
            return result;
        }

        private string FixFilename(string filename) => (WorkingDirectory + filename.Replace(@"\", "/")).TrimStart('/').TrimEnd('/');

        public long GetFileLength(string filename)
        {
            var file = GetEntry(filename) ?? throw new FileNotFoundException(Properties.Resources.FileSystem_ZipFileSystem_EntryNotFound, filename);
            return file.Length;
        }

        private IEnumerable<string> EnumerateFiles(string path, string searchPattern, bool topDirectoryOnly)
        {
            path = FixDirectory(path);
            var searchPatternRegex = new Regex(InMemoryFileSystem.GetFileSearchRegex(searchPattern, forFullPath: false));
            var filesInPath = zipArchive.Entries.Where(e => e.FullName.StartsWith(path));
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

        public void ResetWorkingDirectory()
        {
            WorkingDirectory = "/";
        }

        public bool FileExists(string filename)
        {
            return GetEntry(FixFilename(filename)) != null;
        }

        public bool DirectoryExists(string path)
        {
            var directory = FixDirectory(path);
            return EnumerateDirectories(Path.GetDirectoryName(directory) ?? "", true).Any(d => d == directory);
        }

        public Stream OpenFileReadOnly(string filename)
        {
            var file = GetEntry(filename) ?? throw new FileNotFoundException(Properties.Resources.FileSystem_ZipFileSystem_EntryNotFound, filename);
            return new ReadOnlyStream(file.Open());
        }

        // The following is the remaining implementation for IFileSystem to support writing
        // There's implications that need to be resolved, such as not being able to read an entry's length after opening it for writing
        // For now it's easier to just let zip files be read-only

        // private int tempCounter;
        // private readonly object tempCounterLock = new object();

        //public void CreateDirectory(string path)
        //{
        //    // Do nothing for now
        //    // Zip files can only contain files, and not directories
        //    // In the future, maybe we create a dummy file to preserve
        //}

        //public void CopyFile(string sourceFilename, string destinationFilename)
        //{
        //    using var sourceFile = OpenFileReadOnly(sourceFilename);
        //    using var destinationFile = OpenFileWriteOnly(destinationFilename);
        //}

        //public void DeleteFile(string filename)
        //{
        //    var file = GetEntry(filename);
        //    if (file != null)
        //    {
        //        file.Delete();
        //    }
        //}

        //public void DeleteDirectory(string path)
        //{
        //    foreach (var file in EnumerateFiles(path, "*", topDirectoryOnly: false))
        //    {
        //        DeleteFile(file);
        //    }
        //}

        //public string GetTempFilename()
        //{
        //    string? filename = null;
        //    lock (tempCounterLock)
        //    {
        //        while (filename == null || FileExists(filename))
        //        {
        //            filename = "temp/" + tempCounter++.ToString();
        //        }
        //    }
        //    this.WriteAllBytes(filename, Array.Empty<byte>());
        //    return filename;
        //}

        //public string GetTempDirectory()
        //{
        //    string? dirname = null;
        //    lock (tempCounterLock)
        //    {
        //        while (dirname == null || DirectoryExists(dirname))
        //        {
        //            dirname = "temp/" + tempCounter++.ToString();
        //        }
        //    }
        //    this.CreateDirectory(dirname);
        //    return dirname;
        //}

        //public Stream OpenFile(string filename)
        //{
        //    var file = GetEntry(filename) ?? zipArchive.CreateEntry(FixFilename(filename));
        //    return file.Open();
        //}

        //public Stream OpenFileWriteOnly(string filename)
        //{
        //    var file = GetEntry(filename) ?? zipArchive.CreateEntry(FixFilename(filename));
        //    return new WriteOnlyStream(file.Open());
        //}
    }
}
