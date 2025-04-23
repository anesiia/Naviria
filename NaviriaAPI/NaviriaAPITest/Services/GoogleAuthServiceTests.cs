using Moq;
using NUnit.Framework;
using Google.Apis.Auth;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.IServices.IJwtService;
using NaviriaAPI.Services.AuthServices;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace NaviriaAPI.Tests.Services
{
    //[TestFixture]
    //public class GoogleAuthServiceTests
    //{
    //    private Mock<IUserRepository> _mockUserRepository;
    //    private Mock<IJwtService> _mockJwtService;
    //    private GoogleAuthService _googleAuthService;

    //    [SetUp]
    //    public void Setup()
    //    {
    //        _mockUserRepository = new Mock<IUserRepository>();
    //        _mockJwtService = new Mock<IJwtService>();

    //        var configuration = new Mock<IConfiguration>();
    //        configuration.Setup(config => config["Authentication:Google:WebClientId"]).Returns("fake-google-client-id");

    //        _googleAuthService = new GoogleAuthService(
    //            _mockUserRepository.Object,
    //            _mockJwtService.Object,
    //            configuration.Object
    //        );
    //    }

    //    [Test]
        
    //    public async Task TC01_AuthenticateAsync_ShouldReturnToken_WhenValidIdToken()
    //    {
    //        // Arrange
    //        var validIdToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJodHRwczovL2FjY291bnRzLmdvb2dsZS5jb20iLCJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"; // Mock valid JWT with correct 'iss'
    //        var userEntity = new UserEntity
    //        {
    //            Id = "123",
    //            Email = "johndoe@example.com",
    //            Password = "hashed-password"
    //        };

    //        // Mocking the repository and JWT service
    //        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("johndoe@example.com")).ReturnsAsync(userEntity);
    //        _mockJwtService.Setup(jwt => jwt.GenerateUserToken(userEntity)).Returns("fake-jwt-token");

    //        // Act
    //        var result = await _googleAuthService.AuthenticateAsync(validIdToken);

    //        // Assert
    //        Assert.That(result, Is.EqualTo("fake-jwt-token"));
    //        _mockJwtService.Verify(jwt => jwt.GenerateUserToken(userEntity), Times.Once);
    //    }


    //    [Test]
    //    public void TC02_AuthenticateAsync_ShouldThrowArgumentException_WhenUserNotFound()
    //    {
    //        // Arrange
    //        var idToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"; // Mock valid JWT with RS256 in header
    //        var payload = new GoogleJsonWebSignature.Payload
    //        {
    //            Email = "nonexistentuser@example.com"
    //        };

    //        // Mocking the repository to return null (user not found)
    //        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("nonexistentuser@example.com")).ReturnsAsync((UserEntity?)null);

    //        // Act & Assert
    //        var ex = Assert.ThrowsAsync<ArgumentException>(() => _googleAuthService.AuthenticateAsync(idToken));
    //        Assert.That(ex.Message, Is.EqualTo("User with such email does not exist"));
    //    }
    //}
}
