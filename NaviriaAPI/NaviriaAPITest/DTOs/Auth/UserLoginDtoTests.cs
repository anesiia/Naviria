using NUnit.Framework;
using NaviriaAPI.DTOs.Auth;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NaviriaAPI.Tests.DTOs.Auth
{
    [TestFixture]
    public class UserLoginDtoTests
    {
        // Positive Test Case
        [Test]
        public void TC01_ValidUserLoginDto_ShouldBeValid()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "john.doe@example.com",
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.True);
        }

        // Negative Test Cases for Email
        [Test]
        public void TC02_InvalidEmail_ShouldBeInvalid_WhenEmailIsEmpty()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = string.Empty,
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Email is required")), Is.True);
        }

        [Test]
        public void TC03_InvalidEmail_ShouldBeInvalid_WhenEmailIsInvalidFormat()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "john.doe@com",  // Invalid email format
                Password = "Passw0rd123"
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False); 
        }

        // Negative Test Cases for Password
        [Test]
        public void TC04_InvalidPassword_ShouldBeInvalid_WhenPasswordIsTooShort()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "john.doe@example.com",
                Password = "Short1" // Too short password
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            //Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Password must be at least 8 characters")), Is.True);
        }

        [Test]
        public void TC05_InvalidPassword_ShouldBeInvalid_WhenPasswordLacksUpperCase()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "john.doe@example.com",
                Password = "password123"  // Missing uppercase letter
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Password must contain upper, lower letters and digits")), Is.True);
        }

        [Test]
        public void TC06_InvalidPassword_ShouldBeInvalid_WhenPasswordLacksDigit()
        {
            // Arrange
            var userLoginDto = new UserLoginDto
            {
                Email = "john.doe@example.com",
                Password = "Password"  // Missing digit
            };

            // Act
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(userLoginDto);
            var isValid = Validator.TryValidateObject(userLoginDto, validationContext, validationResults, true);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(validationResults.Any(r => r.ErrorMessage.Contains("Password must contain upper, lower letters and digits")), Is.True);
        }
    }
}