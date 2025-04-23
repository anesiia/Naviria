using NaviriaAPI.DTOs.UpdateDTOs;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPITest.DTOsTests.UpdateDTOs
{
    [TestFixture]
    public class AchievementUpdateDtoTests
    {
        [Test]
        public void Name_ShouldBeValid_WhenValidInput()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Achievement Name",
                Description = "This is a valid description.",
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Is.Empty, "Model validation failed.");
        }

        [Test]
        public void Name_ShouldFailValidation_WhenTooShort()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "A", // Too short, should fail validation
                Description = "Valid description.",
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Name being too short.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Achievement name must be at least 2 characters long."));
        }

        [Test]
        public void Name_ShouldFailValidation_WhenInvalidCharacters()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "InvalidName@123", // Invalid characters, should fail validation
                Description = "Valid description.",
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Name containing invalid characters.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Achievement name can only contain Cyrillic and Latin letters, spaces, apostrophes, and hyphens."));
        }

        [Test]
        public void Description_ShouldBeValid_WhenValidInput()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "This description is perfectly valid.",
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Is.Empty, "Model validation failed.");
        }

        [Test]
        public void Description_ShouldFailValidation_WhenInvalidCharacters()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Invalid description with # characters!", // Invalid characters, should fail validation
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Description containing invalid characters.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Description contains invalid characters."));
        }

        [Test]
        public void Points_ShouldBeValid_WhenInRange()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Valid description.",
                Points = 50 // Valid points
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Is.Empty, "Model validation failed.");
        }

        [Test]
        public void Points_ShouldFailValidation_WhenOutOfRange()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Valid description.",
                Points = -5 // Invalid points, should fail validation
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Points being out of range.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The field Points must be between 0 and 2147483647."));
        }

        private static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }
    }
}