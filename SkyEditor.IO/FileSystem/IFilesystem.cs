using System.IO;

namespace SkyEditor.IO.FileSystem
{
    public interface IFileSystem : IReadOnlyFileSystem
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
        /// Opens a file stream with Write privilages.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A <see cref="Stream"/> that has been opened with write permission for the requested file.</returns>
        Stream OpenFileWriteOnly(string filename);
    }
}
