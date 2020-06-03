using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.FileSystem
{
    public interface IReadOnlyFileSystem
    {
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
        /// Gets the length, in bytes, of the file at the given path.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A long indicating the length in bytes of the file.</returns>
        long GetFileLength(string filename);

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
        /// Opens a file stream with Read privilages.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A <see cref="Stream"/> that has been opened with read permission for the requested file.</returns>
        Stream OpenFileReadOnly(string filename);
    }

    public static class ReadOnlyFileSystemExtensions
    {
        /// <summary>
        /// Reads a file from disk, and returns its contents as a string.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="encoding">Encoding of the text to read</param>
        /// <returns>A string representing the contents of the file.</returns>
        public static string ReadAllText(this IReadOnlyFileSystem fileSystem, string filename, Encoding encoding)
        {
            using var stream = fileSystem.OpenFileReadOnly(filename);
            using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads a file from disk, and returns its contents as a string.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="encoding">Encoding of the text to read</param>
        /// <returns>A string representing the contents of the file.</returns>
        public static async Task<string> ReadAllTextAsync(this IReadOnlyFileSystem fileSystem, string filename, Encoding encoding)
        {
            using var stream = fileSystem.OpenFileReadOnly(filename);
            using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true);
            return await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Reads a file from disk, and returns its contents as a string.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A string representing the contents of the file.</returns>
        public static string ReadAllText(this IReadOnlyFileSystem fileSystem, string filename)
        {
            return ReadAllText(fileSystem, filename, Encoding.UTF8);
        }

        /// <summary>
        /// Reads a file from disk, and returns its contents as a string.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <returns>A string representing the contents of the file.</returns>
        public static async Task<string> ReadAllTextAsync(this IReadOnlyFileSystem fileSystem, string filename)
        {
            return await ReadAllTextAsync(fileSystem, filename, Encoding.UTF8).ConfigureAwait(false);
        }

        // Making ReadAllBytes be an extension method is tricker, because the original .Net code has to deal with determining the file size, which isn't always available: https://github.com/dotnet/runtime/blob/ccf6aedb63c37ea8e10e4f5b5d9d23a69bdd9489/src/libraries/System.IO.FileSystem/src/System/IO/File.cs#L341
    }
}
