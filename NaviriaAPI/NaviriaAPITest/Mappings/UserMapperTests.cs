using NUnit.Framework;
using NaviriaAPI.Mappings;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using System;

namespace NaviriaAPITest.Mappings
{
//    public class UserMapperTests
//    {
//        // Позитивний тест: перетворення Entity → DTO
//        [Test]
//        public void TC01_ToDto_MapsCorrectly()
//        {
//            var entity = new UserEntity
//            {
//                Id = "123",
//                FullName = "John Doe",
//                Gender = "Male",
//                Nickname = "johnny",
//                BirthDate = new DateTime(1990, 1, 1),
//                Description = "A sample user.",
//                Achievements = "Achievement 1, Achievement 2",
//                Email = "john.doe@example.com",
//                Password = "password123",
//                Friends = "friend1, friend2",
//                FutureMessage = "Future message.",
//                Photo = "photo.jpg",
//                Points = 100,
//                LastSeen = DateTime.Now,
//                IsOnline = true,
//                IsProUser = false
//            };

//            var dto = UserMapper.ToDto(entity);

//            Assert.That(dto.Id, Is.EqualTo("123"));
//            Assert.That(dto.FullName, Is.EqualTo("John Doe"));
//            Assert.That(dto.Gender, Is.EqualTo("Male"));
//            Assert.That(dto.Nickname, Is.EqualTo("johnny"));
//            Assert.That(dto.BirthDate, Is.EqualTo(new DateTime(1990, 1, 1)));
//            Assert.That(dto.Description, Is.EqualTo("A sample user."));
//            Assert.That(dto.Email, Is.EqualTo("john.doe@example.com"));
//            Assert.That(dto.Password, Is.EqualTo("password123"));
//            Assert.That(dto.Points, Is.EqualTo(100));
//            Assert.That(dto.IsOnline, Is.EqualTo(true));
//            Assert.That(dto.IsProUser, Is.EqualTo(false));
//        }

//        // Позитивний тест: перетворення DTO → Entity
//        [Test]
//        public void TC02_ToEntity_FromDto_MapsCorrectly()
//        {
//            var dto = new UserDto
//            {
//                Id = "123",
//                FullName = "John Doe",
//                Gender = "Male",
//                Nickname = "johnny",
//                BirthDate = new DateTime(1990, 1, 1),
//                Description = "A sample user.",
//                Achievements = "Achievement 1, Achievement 2",
//                Email = "john.doe@example.com",
//                Password = "password123",
//                Friends = "friend1, friend2",
//                FutureMessage = "Future message.",
//                Photo = "photo.jpg",
//                Points = 100,
//                LastSeen = DateTime.Now,
//                IsOnline = true,
//                IsProUser = false
//            };

//            var entity = UserMapper.ToEntity(dto);

//            Assert.That(entity.Id, Is.EqualTo("123"));
//            Assert.That(entity.FullName, Is.EqualTo("John Doe"));
//            Assert.That(entity.Gender, Is.EqualTo("Male"));
//            Assert.That(entity.Nickname, Is.EqualTo("johnny"));
//            Assert.That(entity.BirthDate, Is.EqualTo(new DateTime(1990, 1, 1)));
//            Assert.That(entity.Description, Is.EqualTo("A sample user."));
//            Assert.That(entity.Email, Is.EqualTo("john.doe@example.com"));
//            Assert.That(entity.Password, Is.EqualTo("password123"));
//            Assert.That(entity.Points, Is.EqualTo(100));
//            Assert.That(entity.IsOnline, Is.EqualTo(true));
//            Assert.That(entity.IsProUser, Is.EqualTo(false));
//        }

//        // Позитивний тест: перетворення CreateDto → Entity
//        [Test]
//        public void TC03_ToEntity_FromCreateDto_MapsCorrectly()
//        {
//            var createDto = new UserCreateDto
//            {
//                FullName = "John Doe",
//                Gender = "Male",
//                Nickname = "johnny",
//                BirthDate = new DateTime(1990, 1, 1),
//                Description = "A sample user.",
//                Achievements = "Achievement 1, Achievement 2",
//                Email = "john.doe@example.com",
//                Password = "password123",
//                Friends = "friend1, friend2",
//                FutureMessage = "Future message.",
//                Photo = "photo.jpg",
//                Points = 100,
//                LastSeen = DateTime.Now,
//                IsOnline = true,
//                IsProUser = false
//            };

//            var entity = UserMapper.ToEntity(createDto);

//            Assert.That(entity.FullName, Is.EqualTo("John Doe"));
//            Assert.That(entity.Gender, Is.EqualTo("Male"));
//            Assert.That(entity.Nickname, Is.EqualTo("johnny"));
//            Assert.That(entity.BirthDate, Is.EqualTo(new DateTime(1990, 1, 1)));
//            Assert.That(entity.Description, Is.EqualTo("A sample user."));
//            Assert.That(entity.Email, Is.EqualTo("john.doe@example.com"));
//            Assert.That(entity.Password, Is.EqualTo("password123"));
//            Assert.That(entity.Points, Is.EqualTo(100));
//            Assert.That(entity.IsOnline, Is.EqualTo(true));
//            Assert.That(entity.IsProUser, Is.EqualTo(false));
//        }

//        [Test]
//public void TC04_ToEntity_ShouldMapUserUpdateDtoToUserEntity_Correctly()
//{
//    // Arrange
//    var dto = new UserUpdateDto {
//        FullName = "John Doe",
//        Gender = "Male",
//        Nickname = "johndoe",
//        Description = "A regular user",
//        Achievements = "Achievement 1, Achievement 2",
//        Email = "johndoe@example.com",
//        Password = "password123",
//        Friends = "friend1, friend2",
//        FutureMessage = "See you in the future!",
//        Photo = "profile_pic.jpg",
//        Points = 100,
//        LastSeen = DateTime.Now,
//        IsOnline = true,
//        IsProUser = false
//    };

//    // Act
//    var result = UserMapper.ToEntity("12345", dto);

//    // Assert
//    Assert.AreEqual("12345", result.Id);
//    Assert.AreEqual(dto.FullName, result.FullName);
//    Assert.AreEqual(dto.Gender, result.Gender);
//    Assert.AreEqual(dto.Nickname, result.Nickname);
//    Assert.AreEqual(dto.Description, result.Description);
//    Assert.AreEqual(dto.Achievements, result.Achievements);
//    Assert.AreEqual(dto.Email, result.Email);
//    Assert.AreEqual(dto.Password, result.Password);
//    Assert.AreEqual(dto.Friends, result.Friends);
//    Assert.AreEqual(dto.FutureMessage, result.FutureMessage);
//    Assert.AreEqual(dto.Photo, result.Photo);
//    Assert.AreEqual(dto.Points, result.Points);
//    Assert.AreEqual(dto.LastSeen, result.LastSeen);
//    Assert.AreEqual(dto.IsOnline, result.IsOnline);
//    Assert.AreEqual(dto.IsProUser, result.IsProUser);
//}

}
