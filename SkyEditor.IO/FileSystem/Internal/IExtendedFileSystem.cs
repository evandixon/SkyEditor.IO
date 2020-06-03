using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.IO.FileSystem.Internal
{
    /// <summary>
    /// An implementation of <see cref="IFileSystem"/> with custom implementations of <see cref="FileSystemExtensions"/> extension methods
    /// </summary>
    public interface IExtendedFileSystem : IFileSystem
    {
        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        /// <param name="encoding">Desired encoding of the resulting binary data.</param>
        void WriteAllText(string filename, string data, Encoding encoding);

        /// <summary>
        /// Writes the given text to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">String containing the data to write to the file.</param>
        /// <param name="encoding">Desired encoding of the resulting binary data.</param>
        Task WriteAllTextAsync(string filename, string data, Encoding encoding);

        /// <summary>
        /// Writes the given data to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">Byte array containing the data to write to the file.</param>
        void WriteAllBytes(string filename, byte[] data);

        /// <summary>
        /// Writes the given data to disk.
        /// </summary>
        /// <param name="filename">Full path of the file.</param>
        /// <param name="data">Byte array containing the data to write to the file.</param>
        Task WriteAllBytesAsync(string filename, byte[] data);
    }
}
