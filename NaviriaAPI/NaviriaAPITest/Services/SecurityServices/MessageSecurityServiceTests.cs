using NaviriaAPI.Exceptions;
using NaviriaAPI.Services.SecurityServices;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.SecurityServices
{
    [TestFixture]
    public class MessageSecurityServiceTests
    {
        private MessageSecurityService _service;
        private Mock<ILogger<MessageSecurityService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<MessageSecurityService>>();
            _service = new MessageSecurityService(_loggerMock.Object);
        }

        [Test]
        public void TC001_Validate_ValidMessage_DoesNotThrowException()
        {
            // Arrange
            var userId = "user001";
            var message = "Hello, how are you today?";

            // Act & Assert
            Assert.That(() => _service.Validate(userId, message), Throws.Nothing);
        }

        [Test]
        public void TC002_Validate_EmptyMessage_DoesNotThrowException()
        {
            // Arrange
            var userId = "user001";
            var message = "   ";

            // Act & Assert
            Assert.That(() => _service.Validate(userId, message), Throws.Nothing);
        }

        [Test]
        public void TC003_Validate_MessageWithDangerousKeyword_ThrowsSuspiciousMessageException()
        {
            // Arrange
            var userId = "user002";
            var message = "This contains a DROP command";

            // Act & Assert
            var ex = Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));
            Assert.That(ex.Message, Does.Contain("suspicious content"));
        }

        [Test]
        public void TC004_Validate_MessageWithDangerousRegexPattern_ThrowsSuspiciousMessageException()
        {
            // Arrange
            var userId = "user003";
            var message = "OR 1=1";

            // Act & Assert
            var ex = Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));
            Assert.That(ex.Message, Does.Contain("suspicious content"));
        }

        [Test]
        public void TC005_Validate_MessageWithScriptTag_ThrowsSuspiciousMessageException()
        {
            // Arrange
            var userId = "user004";
            var message = "<script>alert('hack')</script>";

            // Act & Assert
            var ex = Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));
            Assert.That(ex.Message, Does.Contain("suspicious content"));
        }

        [Test]
        public void TC006_Validate_MessageWithSqlInjection_ThrowsSuspiciousMessageException()
        {
            // Arrange
            var userId = "user005";
            var message = "SELECT * FROM users WHERE name = 'admin' --";

            // Act & Assert
            var ex = Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));
            Assert.That(ex.Message, Does.Contain("suspicious content"));
        }

        [Test]
        public void TC007_Validate_DangerousMessage_LogsWarning()
        {
            var userId = "userLogTest";
            var message = "DROP TABLE users";

            Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Suspicious message blocked")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public void TC008_Validate_MessageContainingPartialKeyword_ThrowsIfContainsKeyword()
        {
            var userId = "userPartialKeyword";
            var message = "description"; // містить 'script'

            var ex = Assert.Throws<SuspiciousMessageException>(() => _service.Validate(userId, message));
            Assert.That(ex.Message, Does.Contain("suspicious content"));
        }


    }
}
