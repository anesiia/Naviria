using NUnit.Framework;
using NaviriaAPI.Mappings;
using NaviriaAPI.Entities;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using System;

namespace NaviriaAPITest.Mappings
{
    public class AchievementMapperTests
    {
        [Test]
        public void TC01_ToDto_MapsCorrectly()
        {
            var entity = new AchievementEntity
            {
                Id = "123",
                Name = "Winner",
                Description = "Completed all tasks"
            };

            var dto = AchievementMapper.ToDto(entity);

            Assert.That(dto.Id, Is.EqualTo("123"));
            Assert.That(dto.Name, Is.EqualTo("Winner"));
            Assert.That(dto.Description, Is.EqualTo("Completed all tasks"));
        }

        [Test]
        public void TC02_ToEntity_FromDto_MapsCorrectly()
        {
            var dto = new AchievementDto
            {
                Id = "456",
                Name = "Hero",
                Description = "Saved the world"
            };

            var entity = AchievementMapper.ToEntity(dto);

            Assert.That(entity.Id, Is.EqualTo("456"));
            Assert.That(entity.Name, Is.EqualTo("Hero"));
            Assert.That(entity.Description, Is.EqualTo("Saved the world"));
        }

        [Test]
        public void TC03_ToEntity_FromCreateDto_MapsCorrectly()
        {
            var createDto = new AchievementCreateDto
            {
                Name = "Explorer",
                Description = "Visited all pages"
            };

            var entity = AchievementMapper.ToEntity(createDto);

            Assert.That(entity.Id, Is.Empty); // ID не повинен заповнюватись
            Assert.That(entity.Name, Is.EqualTo("Explorer"));
            Assert.That(entity.Description, Is.EqualTo("Visited all pages"));
        }

        [Test]
        public void TC04_ToEntity_FromUpdateDto_MapsCorrectly()
        {
            var updateDto = new AchievementUpdateDto
            {
                Name = "Champion",
                Description = "Won the contest"
            };

            var entity = AchievementMapper.ToEntity("789", updateDto);

            Assert.That(entity.Id, Is.EqualTo("789"));
            Assert.That(entity.Name, Is.EqualTo("Champion"));
            Assert.That(entity.Description, Is.EqualTo("Won the contest"));
        }

        //[Test]
        //public void TC05_ToDto_NullEntity_ThrowsException()
        //{
        //    Assert.Throws<ArgumentNullException>(() => AchievementMapper.ToDto(null!));
        //}

        //[Test]
        //public void TC06_ToEntity_FromDto_NullDto_ThrowsException()
        //{
        //    Assert.Throws<ArgumentNullException>(() => AchievementMapper.ToEntity((AchievementDto)null!));
        //}

        //[Test]
        //public void TC07_ToEntity_FromCreateDto_NullDto_ThrowsException()
        //{
        //    Assert.Throws<ArgumentNullException>(() => AchievementMapper.ToEntity((AchievementCreateDto)null!));
        //}

        //[Test]
        //public void TC08_ToEntity_FromUpdateDto_NullDto_ThrowsException()
        //{
        //    Assert.Throws<ArgumentNullException>(() => AchievementMapper.ToEntity("123", null!));
        //}

        //[Test]
        //public void TC09_ToEntity_FromUpdateDto_NullId_ThrowsException()
        //{
        //    var updateDto = new AchievementUpdateDto
        //    {
        //        Name = "Achiever",
        //        Description = "Did something great"
        //    };

        //    Assert.Throws<ArgumentNullException>(() => AchievementMapper.ToEntity(null!, updateDto));
        //}
    }
}
