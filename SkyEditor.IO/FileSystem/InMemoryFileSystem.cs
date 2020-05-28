using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SkyEditor.IO.FileSystem
{
    public class InMemoryFileSystem : IFileSystem
    {
        /// <summary>
        /// Gets a regular expression for the given search pattern for use with <see cref="GetFiles(string, string, bool)"/>.  Do not provide asterisks.
        /// </summary>
        public static StringBuilder GetFileSearchRegexQuestionMarkOnly(string searchPattern)
        {
            var parts = searchPattern.Split('?');
            var regexString = new StringBuilder();
            foreach (var item in parts)
            {
                regexString.Append(Regex.Escape(item));
                if (item != parts[parts.Length - 1])
                {
                    regexString.Append(".?");
                }
            }
            return regexString;
        }

        /// <summary>
        /// Gets a regular expression for the given search pattern for use with <see cref="GetFiles(string, string, bool)"/>.
        /// </summary>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static string GetFileSearchRegex(string searchPattern)
        {
            var asteriskParts = searchPattern.Split('*');
            var regexString = new StringBuilder(@"(.*)\/");

            foreach (var part in asteriskParts)
            {
                if (string.IsNullOrEmpty(part))
                {
                    // Asterisk
                    regexString.Append(".*");
                }
                else
                {
                    regexString.Append(GetFileSearchRegexQuestionMarkOnly(part));
                }
            }

            return regexString.ToString();
        }

        public InMemoryFileSystem()
        {
            Files = new ConcurrentDictionary<string, byte[]?>();
            tempCounter = 0;
            WorkingDirectory = "/";
        }

        private int tempCounter;
        private readonly object tempCounterLock = new object();

        protected ConcurrentDictionary<string, byte[]?> Files { get; set; }

        public string WorkingDirectory { get; set; }

        public void ResetWorkingDirectory()
        {
            WorkingDirectory = "/";
        }

        /// <summary>
        /// Standardizes the path, making it absolute if not already
        /// </summary>
        /// <param name="path">The path to standardize.  Can be relative to the working directory (<see cref="WorkingDirectory"/>) or absolute</param>
        /// <returns>The standardized absolute path</returns>
        protected string FixPath(string path)
        {
            var fixedPath = path.Replace('\\', '/');
            if (fixedPath.StartsWith("/"))
            {
                return fixedPath;
            }
            else
            {
                return WorkingDirectory.Replace('\\', '/').TrimEnd('/') + "/" + fixedPath;
            }
        }

        #region IIO Provider Implementation

        public virtual long GetFileLength(string filename)
        {
            var filenameLower = FixPath(filename.ToLowerInvariant());
            var length = Files.FirstOrDefault(x => x.Key.ToLowerInvariant() == filenameLower && x.Value != null).Value?.Length;
            if (!length.HasValue)
            {
                throw new FileNotFoundException(Properties.Resources.FileSystem_InMemoryFileSystem_FileNotFound, filename);
            }

            return length.Value;
        }

        public virtual bool FileExists(string filename)
        {
            var filenameLower = FixPath(filename.ToLowerInvariant());
            return Files.Any(x => x.Key.ToLower() == filenameLower && x.Value != null);
        }

        public virtual bool DirectoryExists(string path)
        {
            var dirNameLower = FixPath(path.ToLowerInvariant());
            return Files.Any(x => x.Key.ToLower() == dirNameLower && x.Value == null);
        }

        public virtual void CreateDirectory(string path)
        {
            path = FixPath(path);
            if (!string.IsNullOrEmpty(path))
            {
                // Create the parent directory
                var parentPath = Path.GetDirectoryName(path)?.Replace(@"\", @"/");
                if (!string.IsNullOrEmpty(parentPath) && !DirectoryExists(parentPath!))
                {
                    CreateDirectory(parentPath!);
                }

                // Create the directory
                Files[path] = null;
            }
        }

        public virtual string[] GetFiles(string path, string searchPattern, bool topDirectoryOnly)
        {
            path = FixPath(path).ToLowerInvariant().TrimEnd('/') + "/";
            var searchPatternRegex = new Regex(GetFileSearchRegex(searchPattern));
            var filesInPath = Files.Where(x => x.Key.ToLowerInvariant().StartsWith(path));
            if (topDirectoryOnly)
            {
                var slashCount = path.Count(x => x == '/');
                filesInPath = filesInPath.Where(x => x.Key.Count(y => y == '/') == slashCount);
            }
            return filesInPath.Where(x => searchPatternRegex.IsMatch(x.Key) && x.Value != null).Select(x => x.Key).ToArray();
        }

        public string[] GetDirectories(string path, bool topDirectoryOnly)
        {
            var pathLower = FixPath(path.ToLowerInvariant() + "/");
            return Files.Where(x => x.Key.ToLowerInvariant().StartsWith(pathLower) && x.Value == null).Select(x => x.Key).ToArray();
        }

        public byte[] ReadAllBytes(string filename)
        {
            var filenameLower = FixPath(filename.ToLower());
            return Files.FirstOrDefault(x => x.Key.ToLowerInvariant() == filenameLower && x.Value != null).Value ?? throw new FileNotFoundException(Properties.Resources.FileSystem_InMemoryFileSystem_FileNotFound, filename);
        }

        public void WriteAllBytes(string filename, byte[] data)
        {
            Files[FixPath(filename)] = data;
        }

        public void CopyFile(string sourceFilename, string destinationFilename)
        {
            WriteAllBytes(FixPath(destinationFilename), ReadAllBytes(FixPath(sourceFilename)));
        }

        public void DeleteFile(string filename)
        {
            var filenameLower = FixPath(filename.ToLowerInvariant());
            foreach (var match in Files.Where(x => x.Key.ToLowerInvariant() == filenameLower && x.Value != null).ToList())
            {
                Files.TryRemove(match.Key, out _);
            }
        }

        public void DeleteDirectory(string path)
        {
            path = FixPath(path);
            if (DirectoryExists(path))
            {
                // Delete child directories
                var pathStart = path.ToLowerInvariant() + "/";
                foreach (var item in Files.Where(x => x.Key.ToLowerInvariant().StartsWith(pathStart)).ToList())
                {
                    Files.TryRemove(item.Key, out _);
                }

                // Delete the directory
                Files.TryRemove(Files.FirstOrDefault(x => x.Key.ToLowerInvariant() == path.ToLowerInvariant() && x.Value == null).Key, out _);
            }
        }

        public string GetTempFilename()
        {
            string? filename = null;
            lock (tempCounterLock)
            {
                while (filename == null || FileExists(filename))
                {
                    filename = "/temp/" + tempCounter++.ToString();
                }
            }
            WriteAllBytes(filename, Array.Empty<byte>());
            return filename;
        }

        public string GetTempDirectory()
        {
            string? dirname = null;
            lock (tempCounterLock)
            {
                while (dirname == null || DirectoryExists(dirname))
                {
                    dirname = "/temp/" + tempCounter++.ToString();
                }
            }
            CreateDirectory(dirname);
            return dirname;
        }

        public Stream OpenFile(string filename)
        {
            if (!FileExists(filename))
            {
                WriteAllBytes(filename, new byte[] { });
            }
            return new MemoryStream(ReadAllBytes(FixPath(filename)), true);
        }

        public Stream OpenFileReadOnly(string filename)
        {
            if (!FileExists(filename))
            {
                WriteAllBytes(filename, new byte[] { });
            }
            return new MemoryStream(ReadAllBytes(FixPath(filename)), false);
        }

        public Stream OpenFileWriteOnly(string filename)
        {
            if (!FileExists(filename))
            {
                WriteAllBytes(filename, new byte[] { });
            }
            return new MemoryStream(ReadAllBytes(FixPath(filename)), true);
        }

        #endregion
    }
}
