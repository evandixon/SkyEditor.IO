using FluentAssertions;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Xunit;

namespace SkyEditor.IO.Tests.FileSystem
{
    public class ZipFileSystemTests : IDisposable
    {
        public ZipFileSystemTests()
        {
            tempFileName = Path.GetTempFileName();
            File.Delete(tempFileName); // ZipFile.CreateFromDirectory can't overwrite files
            ZipFile.CreateFromDirectory("TestData", tempFileName);
            archiveStream = File.Open(tempFileName, FileMode.Open);
            archive = new ZipArchive(archiveStream, ZipArchiveMode.Update);
        }

        private readonly string tempFileName;
        private readonly Stream archiveStream;
        private readonly ZipArchive archive;

        public void Dispose()
        {
            archiveStream?.Dispose();
            if (File.Exists(tempFileName))
            {
                File.Delete(tempFileName);
            }
        }

        [Fact]
        public void CanReadExistingFiles()
        {
            // Arrange
            var fileSystem = new ZipFileSystem(archive);

            // Act
            var data = fileSystem.ReadAllText("Directory1/TextFile1.txt");

            // Assert
            data.Should().BeEquivalentTo("~/Directory1/TextFile1");
        }

        [Fact]
        public void FileExistsNegativeTest()
        {
            var provider = new ZipFileSystem(archive);
            Assert.False(provider.FileExists(""), "No files should exist.");
            Assert.False(provider.FileExists("/temp/0"), "No files should exist.");
            Assert.False(provider.FileExists("/directory"), "No files should exist.");
        }

        [Fact]
        public void DirectoryExistsNegativeTest()
        {
            var provider = new ZipFileSystem(archive);
            Assert.False(provider.DirectoryExists("/temp/0"), "No directories should exist.");
            Assert.False(provider.DirectoryExists("/directory"), "No directories should exist.");
        }

        [Fact]
        public void GetFiles()
        {
            var provider = new ZipFileSystem(archive);

            var allDir1 = provider.GetFiles("/Directory1", "*", false);
            allDir1.Length.Should().Be(1);
            allDir1[0].Should().Be("Directory1/TextFile1.txt");

            var antiRecursionTxt = provider.GetFiles("/", "*.txt", true);
            antiRecursionTxt.Length.Should().Be(1);
            antiRecursionTxt[0].Should().Be("TextFile1.txt");

            var rootFiles = provider.GetFiles("/", "TextFile?.txt", true);
            rootFiles.Length.Should().Be(1);
            rootFiles[0].Should().Be("TextFile1.txt");
        }

        // The following tests are for the commented-out write support
        // Uncomment if and when ZipFileSystem gets write support

        //[Fact]
        //public void CreateDirectory()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    provider.CreateDirectory("/directory");
        //    Assert.True(provider.DirectoryExists("/directory"), "Directory \"/directory\" not created");

        //    provider.CreateDirectory("/directory/subDirectory");
        //    Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Directory \"/directory/subDirectory\" not created");
        //}

        //[Fact]
        //public void CreateDirectoryRecursive()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    provider.CreateDirectory("/root/directory");
        //    if (!provider.DirectoryExists("/root/directory"))
        //    {
        //        throw new Exception("Directory /root/directory not created.");
        //    }
        //    Assert.True(provider.DirectoryExists("/root"), "Directory \"/root\" not created when \"/root/directory\" was created.");
        //}

        //[Fact]
        //public void ByteReadWrite()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //    provider.WriteAllBytes("/testFile.bin", testSequence);

        //    var read = provider.ReadAllBytes("/testFile.bin");
        //    Assert.True(testSequence.SequenceEqual(read));
        //}

        //[Fact]
        //public void TextReadWrite()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    string testSequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZqbcdefghijklmnopqrstuvwxyz0123456789àèéêç";
        //    provider.WriteAllText("/testFile.bin", testSequence);

        //    var read = provider.ReadAllText("/testFile.bin");
        //    Assert.True(testSequence.SequenceEqual(read));
        //}

        //[Fact]
        //public void FileLength()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //    provider.WriteAllBytes("/testFile.bin", testSequence);

        //    Assert.Equal(Convert.ToInt64(testSequence.Length), provider.GetFileLength("/testFile.bin"));
        //}

        //[Fact]
        //public void DeleteDirectory()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    provider.CreateDirectory("/directory/subDirectory");
        //    provider.CreateDirectory("/test/directory");
        //    if (!provider.DirectoryExists("/directory") || !provider.DirectoryExists("/test"))
        //    {
        //        throw new Exception("Couldn't create test directory");
        //    }

        //    provider.DeleteDirectory("/test/directory");
        //    Assert.False(provider.DirectoryExists("/test/directory"), "Directory \"/test/directory\" not deleted.");
        //    Assert.True(provider.DirectoryExists("/test"), "Incorrect directory deleted: \"/test\"");
        //    Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Incorrect directory deleted: \"/directory/subDirectory\"");
        //    Assert.True(provider.DirectoryExists("/directory"), "Incorrect directory deleted: \"/directory\"");
        //}

        //[Fact]
        //public void DeleteDirectoryRecursive()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    provider.CreateDirectory("/directory/subDirectory");
        //    provider.CreateDirectory("/test/directory");
        //    if (!provider.DirectoryExists("/directory") || !provider.DirectoryExists("/test"))
        //    {
        //        throw new Exception("Couldn't create test directory");
        //    }

        //    provider.DeleteDirectory("/test");
        //    Assert.False(provider.DirectoryExists("/test/directory"), "Directory \"/test/directory\" not deleted recursively.");
        //    Assert.False(provider.DirectoryExists("/test"), "Directory \"/test\" not deleted.");
        //    Assert.True(provider.DirectoryExists("/directory/subDirectory"), "Incorrect directory deleted: \"/directory/subDirectory\"");
        //    Assert.True(provider.DirectoryExists("/directory"), "Incorrect directory deleted: \"/directory\"");
        //}

        //[Fact]
        //public void DeleteFile()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //    provider.WriteAllBytes("/testFile.bin", testSequence);

        //    if (!provider.FileExists("/testFile.bin"))
        //    {
        //        throw new Exception("Unable to create test file.");
        //    }

        //    provider.DeleteFile("/testFile.bin");

        //    Assert.False(provider.FileExists("/testFile.bin"), "File not deleted.");
        //}

        //[Fact]
        //public void CopyFile()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    byte[] testSequence = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //    provider.WriteAllBytes("/testFile.bin", testSequence);

        //    if (!provider.FileExists("/testFile.bin"))
        //    {
        //        throw new Exception("Unable to create test file.");
        //    }

        //    provider.CopyFile("/testFile.bin", "/testFile2.bin");

        //    Assert.True(provider.FileExists("/testFile2.bin"), "File not copied.");
        //    Assert.Equal(testSequence, provider.ReadAllBytes("/testFile2.bin"));
        //}

        //[Fact]
        //public void GetTempFilename_NotNull()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    var tempFilename = provider.GetTempFilename();
        //    Assert.NotNull(tempFilename);
        //}

        //[Fact]
        //public void GetTempFilename_FileExists()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    var tempFilename = provider.GetTempFilename();
        //    Assert.True(provider.FileExists(tempFilename), "Temp file does not exist");
        //}

        //[Fact]
        //public void GetTempDirectory_NotNull()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    var tempDirectory = provider.GetTempDirectory();
        //    Assert.NotNull(tempDirectory);
        //}

        //[Fact]
        //public void GetTempDirectory_FileExists()
        //{
        //    var provider = new ZipFileSystem(archive);
        //    var tempDirectory = provider.GetTempDirectory();
        //    Assert.True(provider.DirectoryExists(tempDirectory), "Temp directory does not exist");
        //}
    }
}
