using Moq;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.SignalRHub;
using NUnit.Framework;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace NaviriaAPITest.ServicesTests
{
    //[TestFixture]
    //public class PresenceHubTests
    //{
    //    private Mock<IUserRepository> _mockUserRepository;
    //    private Mock<HubCallerContext> _mockContext;
    //    private PresenceHub _presenceHub;

    //    [SetUp]
    //    public void Setup()
    //    {
    //        // Create mock repository
    //        _mockUserRepository = new Mock<IUserRepository>();

    //        // Create mock HubCallerContext
    //        _mockContext = new Mock<HubCallerContext>();

    //        // Mock Context.User to simulate a logged-in user
    //        _mockContext.Setup(c => c.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
    //        {
    //            new Claim(ClaimTypes.NameIdentifier, "user123")
    //        })));

    //        // Create the PresenceHub instance and assign its Context property
    //        _presenceHub = new PresenceHub(_mockUserRepository.Object)
    //        {
    //            Context = _mockContext.Object
    //        };
    //    }

    //    [Test]
    //    public async Task OnConnectedAsync_ShouldUpdatePresence_WhenUserIsLoggedIn()
    //    {
    //        // Act
    //        await _presenceHub.OnConnectedAsync();

    //        // Assert: Verify that UpdatePresenceAsync was called with the correct parameters
    //        _mockUserRepository.Verify(repo => repo.UpdatePresenceAsync("user123", It.IsAny<DateTime>(), true), Times.Once);
    //    }

    //    [Test]
    //    public async Task OnConnectedAsync_ShouldNotUpdatePresence_WhenUserIdIsNull()
    //    {
    //        // Arrange: Set Context.User to null to simulate no user
    //        _mockContext.Setup(c => c.User).Returns((ClaimsPrincipal?)null);

    //        // Act
    //        await _presenceHub.OnConnectedAsync();

    //        // Assert: Verify that UpdatePresenceAsync is not called
    //        _mockUserRepository.Verify(repo => repo.UpdatePresenceAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Never);
    //    }

    //    [Test]
    //    public async Task OnDisconnectedAsync_ShouldUpdatePresence_WhenUserIsLoggedIn()
    //    {
    //        // Act
    //        await _presenceHub.OnDisconnectedAsync(null);

    //        // Assert: Verify that UpdatePresenceAsync was called with the correct parameters
    //        _mockUserRepository.Verify(repo => repo.UpdatePresenceAsync("user123", It.IsAny<DateTime>(), false), Times.Once);
    //    }

    //    [Test]
    //    public async Task OnDisconnectedAsync_ShouldNotUpdatePresence_WhenUserIdIsNull()
    //    {
    //        // Arrange: Set Context.User to null to simulate no user
    //        _mockContext.Setup(c => c.User).Returns((ClaimsPrincipal?)null);

    //        // Act
    //        await _presenceHub.OnDisconnectedAsync(null);

    //        // Assert: Verify that UpdatePresenceAsync is not called
    //        _mockUserRepository.Verify(repo => repo.UpdatePresenceAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<bool>()), Times.Never);
    //    }

    //    // Add a TearDown method to clean up after each test
    //    [TearDown]
    //    public void TearDown()
    //    {
    //        // Here you can dispose of any objects if needed or set references to null
    //        _presenceHub = null;  // Set to null to allow garbage collection
    //    }
    //}
}
