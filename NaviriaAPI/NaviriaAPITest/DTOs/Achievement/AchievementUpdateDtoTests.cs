using NaviriaAPI.DTOs.Achievement;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace NaviriaAPI.Tests.DTOs.Achievement
{
    [TestFixture]
    public class AchievementUpdateDtoTests
    {
        [Test]
        public void TC001_Name_ShouldBeValid_WhenValidInput()
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
        public void TC002_Name_ShouldFailValidation_WhenTooShort()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "A", 
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
        public void TC003_Name_ShouldFailValidation_WhenInvalidCharacters()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "InvalidName@123", // @
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
        public void TC004_Description_ShouldBeValid_WhenValidInput()
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
        public void TC005_Description_ShouldFailValidation_WhenInvalidCharacters()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Invalid description with # characters!", 
                Points = 10
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Description containing invalid characters.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("Description contains invalid characters."));
        }

        [Test]
        public void TC006_Points_ShouldBeValid_WhenInRange()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Valid description.",
                Points = 50 
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Is.Empty, "Model validation failed.");
        }

        [Test]
        public void TC007_Points_ShouldFailValidation_WhenOutOfRange()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = "Valid description.",
                Points = -5 
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Count.EqualTo(1), "Expected validation error for Points being out of range.");
            Assert.That(validationResults[0].ErrorMessage, Is.EqualTo("The field Points must be between 0 and 2147483647."));
        }

        [Test]
        public void TC008_Name_ShouldFailValidation_WhenTooLong()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = new string('a', 51), 
                Description = "Valid description.",
                Points = 10,
                IsRare = true
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Exactly(1).Matches<ValidationResult>(
                vr => vr.MemberNames.Contains("Name") && vr.ErrorMessage.Contains("maximum length")),
                "Expected validation error for Name exceeding max length.");
        }

        [Test]
        public void TC009_Description_ShouldFailValidation_WhenTooLong()
        {
            // Arrange
            var dto = new AchievementUpdateDto
            {
                Name = "Valid Name",
                Description = new string('a', 151), 
                Points = 10,
                IsRare = true
            };

            // Act
            var validationResults = ValidateModel(dto);

            // Assert
            Assert.That(validationResults, Has.Exactly(1).Matches<ValidationResult>(
                vr => vr.MemberNames.Contains("Description") && vr.ErrorMessage.Contains("maximum length")),
                "Expected validation error for Description exceeding max length.");
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