using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.IO.FileSystem
{
    public interface IFileSystem
    {
        /// <summary>
        /// The directory used when paths are not absolute
        /// </summary>
        string WorkingDirectory { get; set; }

        /// <summary>
        /// Resets the current working directory to its original value
        /// </summary>
        void ResetWorkingDirectory();

        /// <summary>
        /// Gets the length, in bytes, of the file at the given path.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A long indicating the length in bytes of the file.</returns>
        long GetFileLength(string filename);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A boolean indicating whether or not a file exists at the given path.</returns>
        bool FileExists(string filename);

        /// <summary>
        /// Determines whether the specified directory exists.
        /// </summary>
        /// <param name="path">Full path of the directory.</param>
        /// <returns>A boolean indicating whether or not a directory exists at the given path.</returns>
        bool DirectoryExists(string path);

        /// <summary>
        /// Creates a directory at the given path.
        /// </summary>
        /// <param name="path">Full path of the new directory.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Gets the full paths of the files in the directory at the given path.
        /// </summary>
        /// <param name="path">Full path of the directory from which to get the files.</param>
        /// <param name="searchPattern">The search string to match against the names of files in path. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
        /// <param name="topDirectoryOnly">True to search only the top directory.  False to search all child directories too.</param>
        /// <returns>An array containing the full paths of the files matching the search criteria.</returns>
        string[] GetFiles(string path, string searchPattern, bool topDirectoryOnly);

        /// <summary>
        /// Gets the full paths of the directories in the directory at the given path
        /// </summary>
        /// <param name="path">Full path of the directory from which to get the directories.</param>
        /// <param name="topDirectoryOnly">True to search only the top directory.  False to search all child directories too.</param>
        /// <returns>An array containing the full paths of the directories matching the search criteria.</returns>
        string[] GetDirectories(string path, bool topDirectoryOnly);

        /// <summary>
        /// Reads a file from disk, and returns its contents as a byte array.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>An array of byte representing the contents of the file.</returns>
        byte[] ReadAllBytes(string filename);

        /// <summary>
        /// Reads a file from disk, and returns its contents as a string.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A string representing the contents of the file.</returns>
        string ReadAllText(string filename);

        /// <summary>
        /// Writes the given byte array to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">Array of byte containing the data to write to the file.</param>
        void WriteAllBytes(string filename, byte[] data);

        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        void WriteAllText(string filename, string data);

        /// <summary>
        /// Copies a file, overwriting the destination file if it exists.
        /// </summary>
        /// <param name="sourceFilename">Full path of the file to copy.</param>
        /// <param name="destinationFilename">Full path of where to copy the file.  If a file exists at this path already, it will be overwritten.</param>
        void CopyFile(string sourceFilename, string destinationFilename);

        /// <summary>
        /// Recursively deletes the directory at the given path.
        /// </summary>
        /// <param name="filename">Full path of the file to delete.</param>
        void DeleteFile(string filename);

        /// <summary>
        /// Recursively deletes the directory at the given path.
        /// </summary>
        /// <param name="path">Full path of the directory to delete.</param>
        void DeleteDirectory(string path);

        /// <summary>
        /// Creates a temporary, blank file and returns its full path.
        /// </summary>
        /// <returns>Full path of the temporary file.</returns>
        string GetTempFilename();

        /// <summary>
        /// Creates a temporary empty directory and returns its full path.
        /// </summary>
        /// <returns>Full path of the temporary directory.</returns>
        string GetTempDirectory();

        /// <summary>
        /// Opens a file stream with Read/Write privilages.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A <see cref="Stream"/> that has been opened with read and write permissions for the requested file.</returns>
        Stream OpenFile(string filename);

        /// <summary>
        /// Opens a file stream with Read privilages.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A <see cref="Stream"/> that has been opened with read permission for the requested file.</returns>
        Stream OpenFileReadOnly(string filename);

        /// <summary>
        /// Opens a file stream with Write privilages.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A <see cref="Stream"/> that has been opened with write permission for the requested file.</returns>
        Stream OpenFileWriteOnly(string filename);
    }
}
