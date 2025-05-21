using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.Tests.helper;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NaviriaAPI.Tests.DTOs.CreateDTOs
{
    public class AchievementCreateDtoTests
    {

        [Test]
        public void TC001_ValidAchievement_ShouldPassValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Досягнення 1",
                Description = "Опис досягнення",
                Points = 100,
                IsRare = true
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void TC002_EmptyName_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "",
                Description = "Опис",
                Points = 10,
                IsRare = false
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC003_NameTooShort_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "A",
                Description = "Опис",
                Points = 10
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC004_NameTooLong_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = new string('A', 51), 
                Description = "Опис",
                Points = 10
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC005_NameWithInvalidCharacters_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Achiev@ment!", 
                Description = "Valid",
                Points = 10
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC006_DescriptionTooLong_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = new string('D', 151), 
                Points = 10
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC007_DescriptionWithInvalidCharacters_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = "Invalid 💥 Description",
                Points = 10
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC008_NegativePoints_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto
            {
                Name = "Valid Name",
                Description = "Valid",
                Points = -5
            };

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC009_MissingRequiredFields_ShouldFailValidation()
        {
            var dto = new AchievementCreateDto(); 

            var isValid = ValidationHelper.ValidateModel(dto);

            Assert.That(isValid, Is.False);
        }
    }
}