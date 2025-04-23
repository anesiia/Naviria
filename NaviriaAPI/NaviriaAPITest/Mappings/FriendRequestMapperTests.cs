using NUnit.Framework;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Entities;
using NaviriaAPI.Mappings;

namespace NaviriaAPI.Tests.Mappings
{
    //[TestFixture]
    //public class FriendRequestMapperTests
    //{
        //// Позитивний тест для маппінгу з Entity → DTO
        //[Test]
        //public void ToDto_ShouldReturnCorrectFriendRequestDto_WhenGivenFriendRequestEntity()
        //{
        //    // Arrange
        //    var entity = new FriendRequestEntity
        //    {
        //        Id = "1",
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "Pending"
        //    };

        //    // Act
        //    var dto = FriendRequestMapper.ToDto(entity);

        //    // Assert
            
        //    Assert.That(dto.Id, Is.EqualTo(entity.Id));
        //    Assert.That(dto.FromUserId, Is.EqualTo(entity.FromUserId));
        //    Assert.That(dto.ToUserId, Is.EqualTo(entity.ToUserId));
        //    Assert.That(dto.Status, Is.EqualTo(entity.Status));
        //}

        //// Позитивний тест для маппінгу з DTO → Entity
        //[Test]
        //public void ToEntity_ShouldReturnCorrectFriendRequestEntity_WhenGivenFriendRequestDto()
        //{
        //    // Arrange
        //    var dto = new FriendRequestDto
        //    {
        //        Id = "1",
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "Pending"
        //    };

        //    // Act
        //    var entity = FriendRequestMapper.ToEntity(dto);

        //    // Assert
        //    Assert.That(entity.Id, Is.EqualTo(dto.Id));
        //    Assert.That(entity.FromUserId, Is.EqualTo(dto.FromUserId));
        //    Assert.That(entity.ToUserId, Is.EqualTo(dto.ToUserId));
        //    Assert.That(entity.Status, Is.EqualTo(dto.Status));
        //}

        //// Позитивний тест для маппінгу з CreateDto → Entity
        //[Test]
        //public void ToEntity_ShouldReturnCorrectFriendRequestEntity_WhenGivenFriendRequestCreateDto()
        //{
        //    // Arrange
        //    var createDto = new FriendRequestCreateDto
        //    {
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "Pending"
        //    };

        //    // Act
        //    var entity = FriendRequestMapper.ToEntity(createDto);

        //    // Assert
        //    Assert.That(entity.FromUserId, Is.EqualTo(createDto.FromUserId));
        //    Assert.That(entity.ToUserId, Is.EqualTo(createDto.ToUserId));
        //    Assert.That(entity.Status, Is.EqualTo(createDto.Status));
        //    Assert.That(entity.Id, Is.Empty);  // Id не має бути задано при створенні
        //}

        //// Позитивний тест для маппінгу з UpdateDto → Entity
        //[Test]
        //public void ToEntity_ShouldReturnCorrectFriendRequestEntity_WhenGivenFriendRequestUpdateDto()
        //{
        //    // Arrange
        //    var updateDto = new FriendRequestUpdateDto
        //    {
        //        FromUserId = "user1",
        //        ToUserId = "user2",
        //        Status = "Accepted"
        //    };

        //    // Act
        //    var entity = FriendRequestMapper.ToEntity("1", updateDto);

        //    // Assert
        //    Assert.That(entity.Id, Is.EqualTo("1"));  // Перевіряємо, що ID був переданий
        //    Assert.That(entity.FromUserId, Is.EqualTo(updateDto.FromUserId));
        //    Assert.That(entity.ToUserId, Is.EqualTo(updateDto.ToUserId) );
        //    Assert.That(entity.Status, Is.EqualTo(updateDto.Status));
        //}
    //}
}