using NaviriaAPI.DTOs.CreateDTOs;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NaviriaAPITest.DTOsTests.CreateDTOs
{
    public class AchievementCreateDtoTests
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Test]
        public void TC001_ValidAchievement_ShouldPassValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Досягнення дня",
                Description = "Отримано за активну участь.",
                Points = 50,
                IsRecieved = true
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty);  
        }

        [Test]
        public void TC002_EmptyName_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "",
                Description = "Опис",
                Points = 10,
                IsRecieved = true
            };

            var results = ValidateModel(dto);

            Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
        }

        [Test]
        public void TC003_NameTooShort_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "A",
                Description = "Опис",
                Points = 10,
                IsRecieved = true
            };

            var results = ValidateModel(dto);

            Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
        }

        [Test]
        public void TC004_NameWithInvalidCharacters_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Invalid@Name!",
                Description = "Опис",
                Points = 10,
                IsRecieved = false
            };

            var results = ValidateModel(dto);

            Assert.That(results.Any(r => r.MemberNames.Contains("Name")), Is.True);
        }

        [Test]
        public void TC005_DescriptionTooLong_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = new string('a', 151),
                Points = 10,
                IsRecieved = true
            };

            var results = ValidateModel(dto);

           
            Assert.That(results.Any(r => r.MemberNames.Contains("Description")), Is.True);
        }

        [Test]
        public void TC006_DescriptionWithInvalidCharacters_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = "Неправильний опис @#%",
                Points = 15,
                IsRecieved = true
            };

            var results = ValidateModel(dto);

            Assert.That(results.Any(r => r.MemberNames.Contains("Description")), Is.True);
        }

        [Test]
        public void TC007_NegativePoints_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = "Опис",
                Points = -5,
                IsRecieved = false
            };

            var results = ValidateModel(dto);

            Assert.That(results.Any(r => r.MemberNames.Contains("Points")), Is.True);
        }
    }
}