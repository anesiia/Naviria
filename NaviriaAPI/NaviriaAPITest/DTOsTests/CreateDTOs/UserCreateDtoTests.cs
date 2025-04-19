using NUnit.Framework;
using NaviriaAPI.DTOs.CreateDTOs;
using System.ComponentModel.DataAnnotations;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPITest.DTOsTests.CreateDTOs
{
    [TestFixture]
    public class UserCreateDtoTests
    {
        // Positive Test Cases
        [Test]
        public void TC001_UserCreateDto_WithValidData_ShouldBeValid()
        {
            // Arrange
            var dto = new UserCreateDto
            {
                FullName = "Anna Kozlova",
                Nickname = "Anna2025",
                Gender = "f",
                BirthDate = new DateTime(2002, 5, 15),
                Description = "Motivated student",
                Email = "anna.kozlova@example.com",
                Password = "Secure123",
                Points = 100,
                FutureMessage = "Keep pushing!",
                LevelInfo = new LevelProgressInfo { Level = 2, TotalXp = 250 },
                
                Friends = new List<UserFriendInfo>
                {
                        new UserFriendInfo { UserId = "id1", Nickname = "Alice" },
                        new UserFriendInfo { UserId = "id2", Nickname = "Bob" }
                },


                Photo = FileHelper.CreateTestFormFile(),
                Achievements = new List<UserAchievementInfo>
                 {
                        new UserAchievementInfo
                        {
                            AchievementId = "660fd0eccc56d52abc1ef08b",
                            IsReceived = true,
                            ReceivedAt = DateTime.UtcNow
                        }
                 },
                IsOnline = true,
                IsProUser = true,
                LastSeen = DateTime.UtcNow
            };

            // Act
            var context = new ValidationContext(dto, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, context, results, validateAllProperties: true);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }


        // Negative Test Cases for FullName
        [Test]
        public void TC002_InvalidFullName_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "Jo",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("FullName must be at least 3 characters long.")), Is.True);
        }

        [Test]
        public void TC003_InvalidFullName_ShouldBeInvalid_WhenContainsInvalidCharacters()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John @ Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("FullName can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens.")), Is.True);
        }

        // Negative Test Cases for Nickname
        [Test]
        public void TC004_InvalidNickname_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "ab",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Photo = FileHelper.CreateTestFormFile(),
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Nickname must be at least 3 characters long.")), Is.True);
        }

        [Test]
        public void TC005_InvalidNickname_ShouldBeInvalid_WhenContainsInvalidCharacters()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john@123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Photo = FileHelper.CreateTestFormFile(),
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Nickname can only contain Latin letters and digits.")), Is.True);
        }

        // Negative Test Cases for Gender
        [Test]
        public void TC006_InvalidGender_ShouldBeInvalid_WhenNotMOrF()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "x", // Invalid gender
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Gender must be 'f' or 'm'.")), Is.True);
        }

        // Negative Test Cases for Password
        [Test]
        public void TC007_InvalidPassword_ShouldBeInvalid_WhenTooShort()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
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
        public void TC008_InvalidPassword_ShouldBeInvalid_WhenMissingUppercase()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
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
        public void TC009_InvalidEmail_ShouldBeInvalid_WhenIncorrectFormat()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "invalid@email", // Invalid email format
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
        public void TC011_InvalidFullName_ShouldBeInvalid_WhenExceedsMaxLength()
        {
            // Arrange
            var userDto = new UserCreateDto
            {
                FullName = "FFhjgftyhgftyjhgftyujhgfvbghyujklkjhppgfdrtyuilihbq", //50 // Exceeding max length of 50 characters
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
        }

        //Invalid Description with special character
        [Test]
        public void TC012_InvalidDescription_ShouldBeInvalid_WhenContainsInvalidCharacters()
        {
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Password1",
                Description = "Awesome user #1"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, context, results, true);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.ErrorMessage.Contains("Description contains invalid characters.")), Is.True);
        }

        [Test]
        public void TC013_InvalidPoints_ShouldBeInvalid_WhenNegative()
        {
            var userDto = new UserCreateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Gender = "m",
                BirthDate = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Password = "Password1",
                Points = -10
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, context, results, true);

            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("Points")), Is.True);
        }

        [Test]
        public void TC014_UserCreateDto_ShouldBeValid_WithEmptyOptionalFields()
        {
            var userDto = new UserCreateDto
            {
                FullName = "Jane Smith",
                Nickname = "JaneS",
                Gender = "f",
                BirthDate = new DateTime(1995, 6, 20),
                Email = "jane.smith@example.com",
                Password = "ValidPass123",
                Points = 0,
                Description = "",
                FutureMessage = "",
                Achievements = [],
                Friends = [],
                LevelInfo = new LevelProgressInfo(),
                Photo = null
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(userDto);
            var isValid = Validator.TryValidateObject(userDto, context, results, true);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

    }
}
