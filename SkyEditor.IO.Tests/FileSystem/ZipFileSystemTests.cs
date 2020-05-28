using FluentAssertions;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace SkyEditor.IO.Tests.FileSystem
{
    public class ZipFileSystemTests
    {
        [Fact]
        public void CanReadFile()
        {
            var tempFile = Path.GetTempFileName();
            File.Delete(tempFile); // ZipFile.CreateFromDirectory can't overwrite files
            try
            {
                // Arrange
                ZipFile.CreateFromDirectory("TestData", tempFile);
                using var archiveStream = File.OpenRead(tempFile);
                var archive = new ZipArchive(archiveStream);
                var fileSystem = new ZipFileSystem(archive);

                // Act
                var data = fileSystem.ReadAllText("Directory1/TextFile1.txt");

                // Assert
                data.Should().BeEquivalentTo("~/Directory1/TextFile1");
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }
    }
}
