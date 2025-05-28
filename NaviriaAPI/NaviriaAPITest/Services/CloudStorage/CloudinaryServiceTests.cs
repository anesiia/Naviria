using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.CloudStorage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.IO;
using System.Text;
using System;
using System.Threading.Tasks;
using NaviriaAPI.Tests.helper;

namespace NaviriaAPI.Tests.Services.CloudStorage
{
    public class CloudinaryServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IFormFile> _fileMock;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _fileMock = new Mock<IFormFile>();
        }

        [Test]
        public async Task TC01_UploadImageAsync_ShouldReturnTrue_WhenUploadSuccessful()
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("fake image"));
            _fileMock.Setup(f => f.OpenReadStream()).Returns(stream);

            var uploadResult = new ImageUploadResult
            {
                StatusCode = HttpStatusCode.OK,
                SecureUrl = new Uri("https://example.com/photo.jpg")
            };

            _userRepositoryMock
                .Setup(r => r.UpdateProfileImageAsync("user123", "https://example.com/photo.jpg"))
                .ReturnsAsync(true);

            var service = new TestableCloudinaryService(_userRepositoryMock.Object, uploadResult);

            // Act
            var result = await service.UploadImageAsync("user123", _fileMock.Object);

            // Assert
            Assert.That(result, Is.True);
            _userRepositoryMock.Verify(r => r.UpdateProfileImageAsync("user123", "https://example.com/photo.jpg"), Times.Once);
        }

        [Test]
        public void TC02_UploadImageAsync_ShouldThrow_WhenUploadFails()
        {
            // Arrange
            _fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            var failedUpload = new ImageUploadResult
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            var service = new TestableCloudinaryService(_userRepositoryMock.Object, failedUpload);

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.UploadImageAsync("user123", _fileMock.Object));
        }
    }
}