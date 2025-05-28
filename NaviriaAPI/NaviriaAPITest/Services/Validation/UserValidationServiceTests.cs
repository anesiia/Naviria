using Moq;
using NaviriaAPI.DTOs.User;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.Validation;
using NUnit.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Services.Validation
{
    [TestFixture]
    public class UserValidationServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserValidationService _validationService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _validationService = new UserValidationService(_mockUserRepository.Object);
        }

        private UserCreateDto GetValidUserCreateDto() => new UserCreateDto
        {
            Email = "newuser@example.com",
            Nickname = "validNick123",
            FullName = "John Doe",
            Password = "StrongPass1",
            BirthDate = DateTime.UtcNow.AddYears(-25),
            FutureMessage = "This is a valid message."
        };

        [Test]
        public async Task TC01_ShouldThrow_WhenEmailAlreadyExists()
        {
            var dto = GetValidUserCreateDto();
            _mockUserRepository.Setup(r => r.GetByEmailAsync(dto.Email))
                               .ReturnsAsync(new Entities.UserEntity());

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Is.EqualTo("User with this email already exists"));
        }

        [Test]
        public async Task TC02_ShouldThrow_WhenNicknameAlreadyExists()
        {
            var dto = GetValidUserCreateDto();
            _mockUserRepository.Setup(r => r.GetByNicknameAsync(dto.Nickname))
                               .ReturnsAsync(new Entities.UserEntity());

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Is.EqualTo("User with this nickname already exists"));
        }

        [Test]
        public async Task TC03_ShouldThrow_WhenFullNameIsInvalid()
        {
            var dto = GetValidUserCreateDto();
            dto.FullName = "J@ne!";  // invalid characters

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("FullName contains invalid characters"));
        }

        [Test]
        public async Task TC04_ShouldThrow_WhenNicknameIsInvalid()
        {
            var dto = GetValidUserCreateDto();
            dto.Nickname = "in valid!";

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("Nickname must contain only letters and digits"));
        }

        [Test]
        public async Task TC05_ShouldThrow_WhenEmailIsInvalid()
        {
            var dto = GetValidUserCreateDto();
            dto.Email = "invalid_email";

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("Email format is invalid"));
        }

        [Test]
        public async Task TC06_ShouldThrow_WhenPasswordIsInvalid()
        {
            var dto = GetValidUserCreateDto();
            dto.Password = "weak";  // too short

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("Password must be at least 8 characters long"));
        }

        [Test]
        public async Task TC07_ShouldThrow_WhenPasswordMissingUpperLowerDigit()
        {
            var dto = GetValidUserCreateDto();
            dto.Password = "alllowercase";

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("Password must contain at least one uppercase letter"));
        }

        [Test]
        public async Task TC08_ShouldThrow_WhenBirthDateIsInFuture()
        {
            var dto = GetValidUserCreateDto();
            dto.BirthDate = DateTime.UtcNow.AddDays(1);

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Is.EqualTo("Birth date cannot be in the future."));
        }

        [Test]
        public async Task TC09_ShouldThrow_WhenUserIsUnder18()
        {
            var dto = GetValidUserCreateDto();
            dto.BirthDate = DateTime.UtcNow.AddYears(-17);

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Is.EqualTo("User must be at least 18 years old."));
        }

        [Test]
        public async Task TC10_ShouldThrow_WhenUserIsOver120()
        {
            var dto = GetValidUserCreateDto();
            dto.BirthDate = DateTime.UtcNow.AddYears(-121);

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Is.EqualTo("User cannot be older than 120 years."));
        }

        [Test]
        public async Task TC11_ShouldThrow_WhenFutureMessageIsInvalid()
        {
            var dto = GetValidUserCreateDto();
            dto.FutureMessage = "Invalid!!!@###";  // contains invalid characters

            var ex = Assert.ThrowsAsync<ValidationException>(() => _validationService.ValidateCreateAsync(dto));
            Assert.That(ex!.Message, Does.Contain("FutureMessage contains invalid characters"));
        }

        [Test]
        public async Task TC12_ShouldNotThrow_WhenUserIsValid()
        {
            var dto = GetValidUserCreateDto();
            _mockUserRepository.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((Entities.UserEntity?)null);
            _mockUserRepository.Setup(r => r.GetByNicknameAsync(dto.Nickname)).ReturnsAsync((Entities.UserEntity?)null);

            Assert.DoesNotThrowAsync(() => _validationService.ValidateCreateAsync(dto));
        }
    }
}
