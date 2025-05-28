using NUnit.Framework;
using NaviriaAPI.DTOs.User;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using NaviriaAPI.Entities.EmbeddedEntities;

namespace NaviriaAPI.Tests.DTOs.User
{
    [TestFixture]
    public class UserPatchDtoTests
    {
        private UserPatchDto _userPatchDto;

        [SetUp]
        public void Setup()
        {
            _userPatchDto = new UserPatchDto();
        }

        [Test]
        public void TC001_Properties_AssignAndReadValues_Success()
        {
            var levelInfo = new LevelProgressInfo();
            var friends = new List<UserFriendInfo> { new UserFriendInfo() };
            var achievements = new List<UserAchievementInfo> { new UserAchievementInfo() };

            _userPatchDto.FullName = "John Doe";
            _userPatchDto.Nickname = "Johnny";
            _userPatchDto.Description = "A sample description.";
            _userPatchDto.Email = "john.doe@example.com";
            _userPatchDto.Password = "StrongPass123";
            _userPatchDto.Points = 100;
            _userPatchDto.FutureMessage = "Future message";
            _userPatchDto.Photo = "https://example.com/photo.jpg";
            _userPatchDto.IsOnline = true;
            _userPatchDto.IsProUser = false;
            _userPatchDto.LevelInfo = levelInfo;
            _userPatchDto.Friends = friends;
            _userPatchDto.Achievements = achievements;

            Assert.That(_userPatchDto.FullName, Is.EqualTo("John Doe"));
            Assert.That(_userPatchDto.Nickname, Is.EqualTo("Johnny"));
            Assert.That(_userPatchDto.Description, Is.EqualTo("A sample description."));
            Assert.That(_userPatchDto.Email, Is.EqualTo("john.doe@example.com"));
            Assert.That(_userPatchDto.Password, Is.EqualTo("StrongPass123"));
            Assert.That(_userPatchDto.Points, Is.EqualTo(100));
            Assert.That(_userPatchDto.FutureMessage, Is.EqualTo("Future message"));
            Assert.That(_userPatchDto.Photo, Is.EqualTo("https://example.com/photo.jpg"));
            Assert.That(_userPatchDto.IsOnline, Is.True);
            Assert.That(_userPatchDto.IsProUser, Is.False);
            Assert.That(_userPatchDto.LevelInfo, Is.SameAs(levelInfo));
            Assert.That(_userPatchDto.Friends, Is.SameAs(friends));
            Assert.That(_userPatchDto.Achievements, Is.SameAs(achievements));
        }

        [TestCase("Jo", false)]
        [TestCase("J", false)]
        [TestCase("John", true)]
        [TestCase("ThisFullNameIsWayTooLongToBeValidBecauseItExceedsFiftyCharacters", false)]
        public void TC002_FullName_Validation_Works(string fullName, bool expectedIsValid)
        {
            _userPatchDto.FullName = fullName;
            bool isValid = IsPropertyValid(_userPatchDto, nameof(_userPatchDto.FullName));
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }

        [TestCase("ab", false)]
        [TestCase("abc", true)]
        [TestCase("ThisNicknameIsWayTooLongToBeValid", false)]
        public void TC003_Nickname_Validation_Works(string nickname, bool expectedIsValid)
        {
            _userPatchDto.Nickname = nickname;
            bool isValid = IsPropertyValid(_userPatchDto, nameof(_userPatchDto.Nickname));
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }

        [TestCase("notanemail", false)]
        [TestCase("email@example.com", true)]
        public void TC004_Email_Validation_Works(string email, bool expectedIsValid)
        {
            _userPatchDto.Email = email;
            bool isValid = IsPropertyValid(_userPatchDto, nameof(_userPatchDto.Email));
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }

        [TestCase("short", false)]
        [TestCase("longenough", true)]
        public void TC005_Password_Validation_Works(string password, bool expectedIsValid)
        {
            _userPatchDto.Password = password;
            bool isValid = IsPropertyValid(_userPatchDto, nameof(_userPatchDto.Password));
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }

        [TestCase("http:/invalid-url", false)]
        [TestCase("https://valid.url.com/image.png", true)]
        public void TC006_Photo_Validation_Works(string photoUrl, bool expectedIsValid)
        {
            _userPatchDto.Photo = photoUrl;
            bool isValid = IsPropertyValid(_userPatchDto, nameof(_userPatchDto.Photo));
            Assert.That(isValid, Is.EqualTo(expectedIsValid));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        private bool IsPropertyValid(object model, string propertyName)
        {
            var validationResults = ValidateModel(model);
            return !validationResults.Any(vr => vr.MemberNames.Contains(propertyName));
        }
    }
}
