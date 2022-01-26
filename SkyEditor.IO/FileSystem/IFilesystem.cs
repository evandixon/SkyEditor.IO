using SkyEditor.IO.FileSystem.Internal;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        /// Creates a directory at the given path.
        /// </summary>
        /// <param name="path">Full path of the new directory.</param>
        void CreateDirectory(string path);

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

    public static class FileSystemExtensions
    {
        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        public static void WriteAllText(this IFileSystem fileSystem, string filename, string data)
        {
            WriteAllText(fileSystem, filename, data, Encoding.UTF8);
        }

        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        /// <param name="encoding">Desired encoding of the resulting binary data.</param>
        public static void WriteAllText(this IFileSystem fileSystem, string filename, string data, Encoding encoding)
        {
            if (fileSystem is IExtendedFileSystem extendedFileSystem)
            {
                extendedFileSystem.WriteAllText(filename, data, encoding);
                return;
            }

            using var stream = fileSystem.OpenFileWriteOnly(filename);
            using var writer = new StreamWriter(stream, encoding);
            writer.Write(data);
        }

        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        public static async Task WriteAllTextAsync(this IFileSystem fileSystem, string filename, string data)
        {
            await WriteAllTextAsync(fileSystem, filename, data, Encoding.UTF8).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        /// <param name="encoding">Desired encoding of the resulting binary data.</param>
        public static async Task WriteAllTextAsync(this IFileSystem fileSystem, string filename, string data, Encoding encoding)
        {
            if (fileSystem is IExtendedFileSystem extendedFileSystem)
            {
                await extendedFileSystem.WriteAllTextAsync(filename, data, encoding).ConfigureAwait(false);
                return;
            }

            using var stream = fileSystem.OpenFileWriteOnly(filename);
            using var writer = new StreamWriter(stream, encoding);
            await writer.WriteAsync(data).ConfigureAwait(false);
        }

        /// <summary>
        /// Writes the given data to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">Byte array containing the data to write to the file.</param>
        public static void WriteAllBytes(this IFileSystem fileSystem, string filename, byte[] data)
        {
            if (fileSystem is IExtendedFileSystem extendedFileSystem)
            {
                extendedFileSystem.WriteAllBytes(filename, data);
                return;
            }

            using var stream = fileSystem.OpenFileWriteOnly(filename);
            stream.SetLength(data.Length);
            stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Writes the given data to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">Byte array containing the data to write to the file.</param>
        public static async Task WriteAllBytesAsync(this IFileSystem fileSystem, string filename, byte[] data)
        {
            if (fileSystem is IExtendedFileSystem extendedFileSystem)
            {
                await extendedFileSystem.WriteAllBytesAsync(filename, data).ConfigureAwait(false);
                return;
            }

            using var stream = fileSystem.OpenFileWriteOnly(filename);
            stream.SetLength(data.Length);
            await stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }
    }
}
