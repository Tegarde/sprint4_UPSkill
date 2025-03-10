using System;
using System.IO;
using System.Threading.Tasks;
using ForumAPI.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace ForumAPI.Tests.Services
{
    public class FileUploadServiceTests
    {
        private readonly FileUploadService _fileUploadService;

        public FileUploadServiceTests()
        {
            _fileUploadService = new FileUploadService();
        }

        [Fact]
        public async Task UploadFileAsync_ValidFile_ReturnsFilePath()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Act
            var result = await _fileUploadService.UploadFileAsync(fileMock.Object);

            // Assert
            Assert.True(File.Exists(result));
            Assert.EndsWith(".txt", result);
        }

        [Fact]
        public async Task UploadFileAsync_NullFile_ThrowsArgumentException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fileUploadService.UploadFileAsync(null));
        }

        [Fact]
        public async Task UploadFileAsync_EmptyFile_ThrowsArgumentException()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.Length).Returns(0);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fileUploadService.UploadFileAsync(fileMock.Object));
        }
    }
}
