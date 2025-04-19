using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities.EmbeddedEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviriaAPITest.DTOsTests.UpdateDTOs
{
    public class UserUpdateDtoTests
    {
        private static bool ValidateDto(object dto, out List<ValidationResult> results)
        {
            var context = new ValidationContext(dto);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(dto, context, results, true);
        }

        [Test]
        public void TC001_ShouldBeValid_WhenAllFieldsAreCorrect()
        {
            var dto = new UserUpdateDto
            {
                FullName = "John Doe",
                Nickname = "john123",
                Email = "john.doe@example.com",
                Password = "Passw0rd",
                Description = "This is a short description!",
                FutureMessage = "See you in the future.",
                Photo = "https://example.com/photo.jpg",
                Points = 100,
                Friends = new List<UserFriendInfo>
                {
                        new UserFriendInfo { UserId = "id1", Nickname = "Kate" },
                        new UserFriendInfo { UserId = "id2", Nickname = "Bob" }
                },

                Achievements = new List<UserAchievementInfo>
                 {
                        new UserAchievementInfo
                        {
                            AchievementId = "660fd0eccc57685bc1ef08b",
                            IsReceived = true,
                            ReceivedAt = DateTime.UtcNow
                        }
                 },

                LastSeen = DateTime.Now,
                IsOnline = true,
                IsProUser = true
            };

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.True, string.Join(", ", results.Select(r => r.ErrorMessage)));
        }

        [Test]
        public void TC002_ShouldBeInvalid_WhenFullNameTooShort()
        {
            var dto = GetValidDto();
            dto.FullName = "Jo";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("FullName")));
        }

        [Test]
        public void TC003_ShouldBeInvalid_WhenNicknameHasInvalidChars()
        {
            var dto = GetValidDto();
            dto.Nickname = "john@123";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC004_Should_FailValidation_WhenNicknameTooShort()
        {
            var dto = GetValidDto();
            dto.Nickname = "Jo";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
            Assert.That(results.Any(r => r.MemberNames.Contains("Nickname")));
        }

        [Test]
        public void TC005_ShouldBeInvalid_WhenEmailIsWrong()
        {
            var dto = GetValidDto();
            dto.Email = "not-an-email";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC006_ShouldBeInvalid_WhenPasswordTooWeak()
        {
            var dto = GetValidDto();
            dto.Password = "password"; // no uppercase or digit

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC007_ShouldBeInvalid_WhenPointsIsNegative()
        {
            var dto = GetValidDto();
            dto.Points = -10;

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC008_ShouldBeValid_WhenPhotoIsNull()
        {
            // Arrange
            var dto = GetValidDto();  
            dto.Photo = null;  // Встановлюємо поле Photo в null

            // Act
            var isValid = ValidateDto(dto, out var results); 

            // Assert
            Assert.That(isValid, Is.True); 
        }

        [Test]
        public void TC009_ShouldBeInvalid_WhenPhotoIsEmptyString()
        {
            // Arrange
            var dto = GetValidDto();  
            dto.Photo = string.Empty;  // Встановлюємо поле Photo в порожній рядок

            // Act
            var isValid = ValidateDto(dto, out var results);  // Перевірка валідації

            // Assert
            Assert.That(isValid, Is.False);  // Валідація повинна бути неуспішною
            Assert.That(results, Has.Count.GreaterThan(0));  // Перевіряємо, що є помилки валідації
            Assert.That(results[0].MemberNames, Has.One.EqualTo("Photo"));  // Помилка має бути на полі Photo
        }


        [Test]
        public void TC010_ShouldBeInvalid_WhenPhotoIsInvalidUrl()
        {
            var dto = GetValidDto();
            dto.Photo = "not-a-url";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC011_ShouldBeInvalid_WhenDescriptionTooLong()
        {
            var dto = GetValidDto();
            dto.Description = new string('a', 151);

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.False);
        }

        [Test]
        public void TC012_Should_BeValid_WhenFriendsArrayIsEmpty()
        {
            var dto = GetValidDto();
            dto.Friends = new List<UserFriendInfo>();
            //dto.Friends = Array.Empty<string>();

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void TC013_Should_BeValid_WhenAchievementsArrayIsEmpty()
        {
            var dto = GetValidDto();
           dto.Achievements = new List<UserAchievementInfo>();

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void TC014_Should_BeValid_WhenFutureMessageIs2Characters()
        {
            var dto = GetValidDto();
            dto.FutureMessage = "Hi";

            var isValid = ValidateDto(dto, out var results);
            Assert.That(isValid, Is.True);
        }

        private UserUpdateDto GetValidDto()
        {
            return new UserUpdateDto
            {
                FullName = "Jane Doe",
                Nickname = "jane123",
                Email = "jane.doe@example.com",
                Password = "Secure123",
                Description = "Some desc",
                FutureMessage = "Future!",
                Photo = "https://example.com/image.jpg",
                Points = 0,
                Friends = [],
                Achievements = [],
                LastSeen = DateTime.UtcNow,
                IsOnline = false,
                IsProUser = false
            };
        }
    }
}
