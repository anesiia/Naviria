using Moq;
using NaviriaAPI.Services;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.DTOs;
using NaviriaAPI.IRepositories;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using NaviriaAPI.Entities;

namespace NaviriaAPITest.ServicesTests
{
    //internal class FriendRequestServiceTest
    //{
    //    private Mock<IFriendRequestRepository> _mockFriendRequestRepository;
    //    private FriendRequestService _friendRequestService;

    //    [SetUp]
    //    public void SetUp()
    //    {
    //        _mockFriendRequestRepository = new Mock<IFriendRequestRepository>();
    //        _friendRequestService = new FriendRequestService(_mockFriendRequestRepository.Object);
    //    }

    //    // TC-01: Create Friend Request with valid data
    //    [Test]
    //    public async Task TC01_CreateFriendRequest_WithValidData_ReturnsFriendRequestDto()
    //    {
    //        // Arrange
    //        var createDto = new FriendRequestCreateDto
    //        {
    //            FromUserId = "user123",
    //            ToUserId = "user456",
    //            Status = "Pending"
    //        };

    //        var friendRequestEntity = new FriendRequestDto
    //        {
    //            Id = "1",
    //            FromUserId = "user123",
    //            ToUserId = "user456",
    //            Status = "Pending"
    //        };

    //        _mockFriendRequestRepository
    //            .Setup(repo => repo.CreateAsync(It.IsAny<NaviriaAPI.Entities.FriendRequestEntity>()))
    //            .Callback<NaviriaAPI.Entities.FriendRequestEntity>(entity => entity.Id = "1")
    //            .Returns(Task.CompletedTask);

    //        // Act
    //        var result = await _friendRequestService.CreateAsync(createDto);

    //        // Assert
    //        Assert.That(result, Is.Not.Null);
    //        Assert.That(result.Id, Is.EqualTo("1"));
    //        Assert.That(result.FromUserId, Is.EqualTo("user123"));
    //        Assert.That(result.ToUserId, Is.EqualTo("user456"));
    //        Assert.That(result.Status, Is.EqualTo("Pending"));
    //        _mockFriendRequestRepository.Verify(repo => repo.CreateAsync(It.IsAny<NaviriaAPI.Entities.FriendRequestEntity>()), Times.Once);
    //    }

    //    // TC-02: Update Friend Request with valid data
    //    [Test]
    //    public async Task TC02_UpdateFriendRequest_WithValidData_ReturnsTrue()
    //    {
    //        // Arrange
    //        var updateDto = new FriendRequestUpdateDto
    //        {
    //            FromUserId = "user123",
    //            ToUserId = "user456",
    //            Status = "Accepted"
    //        };

    //        _mockFriendRequestRepository
    //            .Setup(repo => repo.UpdateAsync(It.IsAny<NaviriaAPI.Entities.FriendRequestEntity>()))
    //            .ReturnsAsync(true);

    //        // Act
    //        var result = await _friendRequestService.UpdateAsync("1", updateDto);

    //        // Assert
    //        Assert.That(result, Is.True);
    //        _mockFriendRequestRepository.Verify(repo => repo.UpdateAsync(It.IsAny<NaviriaAPI.Entities.FriendRequestEntity>()), Times.Once);
    //    }

    //    [Test]
    //    public async Task TC03_GetFriendRequest_ByValidId_ReturnsFriendRequestDto()
    //    {
    //        // Arrange
    //        var entity = new FriendRequestEntity
    //        {
    //            Id = "1",
    //            FromUserId = "user123",
    //            ToUserId = "user456",
    //            Status = "Pending"
    //        };

    //        _mockFriendRequestRepository.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(entity);

    //        var service = new FriendRequestService(_mockFriendRequestRepository.Object);

    //        // Act
    //        var result = await service.GetByIdAsync("1");

    //        // Assert
    //        Assert.That(result, Is.Not.Null);
    //        Assert.That(result!.Id, Is.EqualTo("1"));
    //        Assert.That(result.FromUserId, Is.EqualTo("user123"));
    //        Assert.That(result.ToUserId, Is.EqualTo("user456"));
    //        Assert.That(result.Status, Is.EqualTo("Pending"));
    //    }


    //    [Test]
    //    public async Task TC04_GetFriendRequest_ByInvalidId_ReturnsNull()
    //    {
    //        // Arrange
    //        _mockFriendRequestRepository.Setup(r => r.GetByIdAsync("invalid-id")).ReturnsAsync((FriendRequestEntity?)null);

    //        var service = new FriendRequestService(_mockFriendRequestRepository.Object);

    //        // Act
    //        var result = await service.GetByIdAsync("invalid-id");

    //        // Assert
    //        Assert.That(result, Is.Null);
    //    }


    //    // TC-05: Delete Friend Request by valid ID
    //    [Test]
    //    public async Task TC05_DeleteFriendRequest_ByValidId_ReturnsTrue()
    //    {
    //        // Arrange
    //        _mockFriendRequestRepository
    //            .Setup(repo => repo.DeleteAsync("1"))
    //            .ReturnsAsync(true);

    //        // Act
    //        var result = await _friendRequestService.DeleteAsync("1");

    //        // Assert
    //        Assert.That(result, Is.True);
    //        _mockFriendRequestRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once);
    //    }

    //    // TC-06: Delete Friend Request by invalid ID
    //    [Test]
    //    public async Task TC06_DeleteFriendRequest_ByInvalidId_ReturnsFalse()
    //    {
    //        // Arrange
    //        _mockFriendRequestRepository
    //            .Setup(repo => repo.DeleteAsync("invalid-id"))
    //            .ReturnsAsync(false);

    //        // Act
    //        var result = await _friendRequestService.DeleteAsync("invalid-id");

    //        // Assert
    //        Assert.That(result, Is.False);
    //        _mockFriendRequestRepository.Verify(repo => repo.DeleteAsync("invalid-id"), Times.Once);
    //    }

    //    [Test]
    //    public async Task TC07_GetAllFriendRequests_ReturnsListOfFriendRequestDtos()
    //    {
    //        // Arrange
    //        var entities = new List<FriendRequestEntity>
    //{
    //    new() { Id = "1", FromUserId = "user123", ToUserId = "user456", Status = "Pending" },
    //    new() { Id = "2", FromUserId = "user789", ToUserId = "user012", Status = "Accepted" }
    //};

    //        _mockFriendRequestRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

    //        var service = new FriendRequestService(_mockFriendRequestRepository.Object);

    //        // Act
    //        var result = (await service.GetAllAsync()).ToList();

    //        // Assert
    //        Assert.That(result.Count, Is.EqualTo(2));

    //        Assert.That(result[0].Id, Is.EqualTo("1"));
    //        Assert.That(result[0].FromUserId, Is.EqualTo("user123"));
    //        Assert.That(result[0].ToUserId, Is.EqualTo("user456"));
    //        Assert.That(result[0].Status, Is.EqualTo("Pending"));

    //        Assert.That(result[1].Id, Is.EqualTo("2"));
    //        Assert.That(result[1].FromUserId, Is.EqualTo("user789"));
    //        Assert.That(result[1].ToUserId, Is.EqualTo("user012"));
    //        Assert.That(result[1].Status, Is.EqualTo("Accepted"));
    //    }

    //}
}
