using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SkyEditor.IO.Tests.FileSystem
{
    public class InMemoryFileSystemTests
    {
        [Fact]
        public void FileExistsNegativeTest()
        {
            var provider = new InMemoryFileSystem();
            Assert.False(provider.FileExists(""), "No files should exist.");
            Assert.False(provider.FileExists("/temp/0"), "No files should exist.");
            Assert.False(provider.FileExists("/directory"), "No files should exist.");
            Assert.False(provider.FileExists("/"), "No files should exist.");
        }

        [Fact]
        public void DirectoryExistsNegativeTest()
        {
            var provider = new InMemoryFileSystem();
            Assert.False(provider.DirectoryExists(""), "No directories should exist.");
            Assert.False(provider.DirectoryExists("/temp/0"), "No directories should exist.");
            Assert.False(provider.DirectoryExists("/directory"), "No directories should exist.");
            Assert.False(provider.DirectoryExists("/"), "No directories should exist.");
        }

        [Fact]
        public void CreateDirectory()
        {
            var provider = new InMemoryFileSystem();
            provider.CreateDirectory("/directory");
            Assert.True(provider.DirectoryExists("/directory"), "Directory \"/directory\" not created");

            provider.CreateDirectory("/directory/subDirectory");
            Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Directory \"/directory/subDirectory\" not created");
        }

        [Fact]
        public void CreateDirectoryRecursive()
        {
            var provider = new InMemoryFileSystem();
            provider.CreateDirectory("/root/directory");
            if (!provider.DirectoryExists("/root/directory"))
            {
                throw new Exception("Directory /root/directory not created.");
            }
            Assert.True(provider.DirectoryExists("/root"), "Directory \"/root\" not created when \"/root/directory\" was created.");
        }

        [Fact]
        public void ByteReadWrite()
        {
            var provider = new InMemoryFileSystem();
            byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            provider.WriteAllBytes("/testFile.bin", testSequence);

            var read = provider.ReadAllBytes("/testFile.bin");
            Assert.True(testSequence.SequenceEqual(read));
        }

        [Fact]
        public void TextReadWrite()
        {
            var provider = new InMemoryFileSystem();
            string testSequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZqbcdefghijklmnopqrstuvwxyz0123456789àèéêç";
            provider.WriteAllText("/testFile.bin", testSequence);

            var read = provider.ReadAllText("/testFile.bin");
            Assert.True(testSequence.SequenceEqual(read));
        }

        [Fact]
        public void FileLength()
        {
            var provider = new InMemoryFileSystem();
            byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            provider.WriteAllBytes("/testFile.bin", testSequence);

            Assert.Equal(Convert.ToInt64(testSequence.Length), provider.GetFileLength("/testFile.bin"));
        }

        [Fact]
        public void DeleteDirectory()
        {
            var provider = new InMemoryFileSystem();
            provider.CreateDirectory("/directory/subDirectory");
            provider.CreateDirectory("/test/directory");
            if (!provider.DirectoryExists("/directory") || !provider.DirectoryExists("/test"))
            {
                throw new Exception("Couldn't create test directory");
            }

            provider.DeleteDirectory("/test/directory");
            Assert.False(provider.DirectoryExists("/test/directory"), "Directory \"/test/directory\" not deleted.");
            Assert.True(provider.DirectoryExists("/test"), "Incorrect directory deleted: \"/test\"");
            Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Incorrect directory deleted: \"/directory/subDirectory\"");
            Assert.True(provider.DirectoryExists("/directory"), "Incorrect directory deleted: \"/directory\"");
        }

        [Fact]
        public void DeleteDirectoryRecursive()
        {
            var provider = new InMemoryFileSystem();
            provider.CreateDirectory("/directory/subDirectory");
            provider.CreateDirectory("/test/directory");
            if (!provider.DirectoryExists("/directory") || !provider.DirectoryExists("/test"))
            {
                throw new Exception("Couldn't create test directory");
            }

            provider.DeleteDirectory("/test");
            Assert.False(provider.DirectoryExists("/test/directory"), "Directory \"/test/directory\" not deleted recursively.");
            Assert.False(provider.DirectoryExists("/test"), "Directory \"/test\" not deleted.");
            Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Incorrect directory deleted: \"/directory/subDirectory\"");
            Assert.True(provider.DirectoryExists("/directory"), "Incorrect directory deleted: \"/directory\"");
        }

        [Fact]
        public void DeleteFile()
        {
            var provider = new InMemoryFileSystem();
            byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            provider.WriteAllBytes("/testFile.bin", testSequence);

            if (!provider.FileExists("/testFile.bin"))
            {
                throw new Exception("Unable to create test file.");
            }

            provider.DeleteFile("/testFile.bin");

            Assert.False(provider.FileExists("/testFile.bin"), "File not deleted.");
        }

        [Fact]
        public void CopyFile()
        {
            var provider = new InMemoryFileSystem();
            byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            provider.WriteAllBytes("/testFile.bin", testSequence);

            if (!provider.FileExists("/testFile.bin"))
            {
                throw new Exception("Unable to create test file.");
            }

            provider.CopyFile("/testFile.bin", "/testFile2.bin");

            Assert.True(provider.FileExists("/testFile2.bin"), "File not copied.");
            Assert.Equal(testSequence, provider.ReadAllBytes("/testFile2.bin"));
        }

        [Fact]
        public void GetFiles()
        {
            var provider = new InMemoryFileSystem();
            provider.WriteAllText("/dir1/file1.txt", "");
            provider.WriteAllText("/dir1/file2.txt", "");
            provider.WriteAllText("/dir1/file3.txt", "");
            provider.WriteAllText("/dir1/file1.bin", "");
            provider.WriteAllText("/dir1/file2.bin", "");
            provider.WriteAllText("/dir1/file3.bin", "");
            provider.WriteAllText("/file1.txt", "");
            provider.WriteAllText("/file2.txt", "");
            provider.WriteAllText("/file3.txt", "");
            provider.WriteAllText("/file1.bin", "");
            provider.WriteAllText("/file2.bin", "");
            provider.WriteAllText("/file3.bin", "");
            provider.WriteAllText("/file.txt", "");

            var allDir1 = provider.GetFiles("/dir1", "*", false);
            var antiRecursionBin = provider.GetFiles("/", "*.bin", true);
            var rootFiles = provider.GetFiles("/", "file?.txt", true);

            Assert.Equal(6, allDir1.Length);
            Assert.Contains(allDir1, x => x == "/dir1/file1.txt");
            Assert.Contains(allDir1, x => x == "/dir1/file2.txt");
            Assert.Contains(allDir1, x => x == "/dir1/file3.txt");
            Assert.Contains(allDir1, x => x == "/dir1/file1.bin");
            Assert.Contains(allDir1, x => x == "/dir1/file2.bin");
            Assert.Contains(allDir1, x => x == "/dir1/file3.bin");

            Assert.Equal(3, antiRecursionBin.Length);
            Assert.Contains(antiRecursionBin, x => x == "/file1.bin");
            Assert.Contains(antiRecursionBin, x => x == "/file2.bin");
            Assert.Contains(antiRecursionBin, x => x == "/file3.bin");

            Assert.Equal(4, rootFiles.Length);
            Assert.Contains(rootFiles, x => x == "/file1.txt");
            Assert.Contains(rootFiles, x => x == "/file2.txt");
            Assert.Contains(rootFiles, x => x == "/file3.txt");
            Assert.Contains(rootFiles, x => x == "/file.txt");
        }

        [Fact]
        public void GetTempFilename_NotNull()
        {
            var provider = new InMemoryFileSystem();
            var tempFilename = provider.GetTempFilename();
            Assert.NotNull(tempFilename);
        }

        [Fact]
        public void GetTempFilename_FileExists()
        {
            var provider = new InMemoryFileSystem();
            var tempFilename = provider.GetTempFilename();
            Assert.True(provider.FileExists(tempFilename), "Temp file does not exist");
        }

        [Fact]
        public void GetTempDirectory_NotNull()
        {
            var provider = new InMemoryFileSystem();
            var tempDirectory = provider.GetTempDirectory();
            Assert.NotNull(tempDirectory);
        }

        [Fact]
        public void GetTempDirectory_FileExists()
        {
            var provider = new InMemoryFileSystem();
            var tempDirectory = provider.GetTempDirectory();
            Assert.True(provider.DirectoryExists(tempDirectory), "Temp directory does not exist");
        }
    }
}
