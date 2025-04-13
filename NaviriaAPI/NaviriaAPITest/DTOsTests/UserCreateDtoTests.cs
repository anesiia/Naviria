using NUnit.Framework;
using NaviriaAPI.DTOs.CreateDTOs;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPITest.DTOsTests
{
    [TestFixture]
    public class UserCreateDtoTests
    {
        // Positive Test Cases
        [Test]
        public void TC01_ValidUserCreateDto_ShouldBeValid()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Description = "Some description",
                Email = "john.doe@example.com",
                Password = "Passw0rd123",
                Points = 0,
                Friends = new string[] { "friend1", "friend2" },
                Achievements = new string[] { "achievement1" },
                FutureMessage = "Future message",
                Photo = "http://example.com/photo.jpg",
                LastSeen = DateTime.Now,
                IsOnline = true,
                IsProUser = false
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.True);
        }

        // Negative Test Cases for FullName
        [Test]
        public void TC02_InvalidFullName_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "Jo",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("FullName must be at least 3 characters long")), Is.True);
        }

        [Test]
        public void TC03_InvalidFullName_ShouldBeInvalid_WhenContainsInvalidCharacters()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John @ Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("FullName can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens")), Is.True);
        }

        // Negative Test Cases for Nickname
        [Test]
        public void TC04_InvalidNickname_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "ab",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Nickname must be at least 3 characters long")), Is.True);
        }

        [Test]
        public void TC05_InvalidNickname_ShouldBeInvalid_WhenContainsInvalidCharacters()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john@123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Nickname can only contain Latin letters and digits")), Is.True);
        }

        // Negative Test Cases for Gender
        [Test]
        public void TC06_InvalidGender_ShouldBeInvalid_WhenNotMOrF()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "x", // Invalid gender
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Gender must be 'f' or 'm'")), Is.True);
        }

        // Negative Test Cases for Password
        [Test]
        public void TC07_InvalidPassword_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Pass1" // Too short (less than 8 characters)
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            /*Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Password must be at least 8 characters long")), Is.True);*/ // Checking the error message for minimum length
        }

        [Test]
        public void TC08_InvalidPassword_ShouldBeInvalid_WhenMissingUppercase()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "password123" // Missing uppercase letter
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Password must contain at least one uppercase letter, one lowercase letter, and one digit")), Is.True);
        }

        // Negative Test Cases for Email
        [Test]
        public void TC09_InvalidEmail_ShouldBeInvalid_WhenIncorrectFormat()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "invalid@email", // Invalid email format
                Password = "Passw0rd123",
                Photo = "http://example.com/photo.jpg"

            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
        }


        [Test]
        public void TC010_ValidFullName_ShouldBeValid_WhenMaxLength()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "FFhjgftyhgftyjhgftyujhgfvbghyujklkjhppgfdrtyuilihb", //50
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Description = "Some description",
                Email = "john.doe@example.com",
                Password = "Passw0rd123",
                FutureMessage = "Future message",
                Photo = "http://example.com/photo.jpg",
                LastSeen = DateTime.Now,
                IsOnline = true,
                IsProUser = false
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void TC11_InvalidFullName_ShouldBeInvalid_WhenExceedsMaxLength()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "FFhjgftyhgftyjhgftyujhgfvbghyujklkjhppgfdrtyuilihbq", //50 // Exceeding max length of 50 characters
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = "http://example.com/photo.jpg",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
        }


        [Test]

        public void TC12_ShouldBeValid_WhenPhotoIsMissing()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
                // Photo is missing
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.True);

        }


        [Test]
        public void TC13_ShouldBeValid_WhenPhotoIsValidUrl()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123",
                Photo = "http://example.com/photo.jpg"  // Valid URL
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void TC14_ShouldBeInvalid_WhenPhotoIsInvalidUrl()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123",
                Photo = "invalid-url"  // Invalid URL
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(v => v.ErrorMessage.Contains("Photo")), Is.True);
        }


    }
}
