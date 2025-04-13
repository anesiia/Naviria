using Moq;
using NUnit.Framework;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.SignalRHub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests
{
    //[TestFixture]
    //public class PresenceHubTests
    //{
    //    private Mock<IUserRepository> _userRepositoryMock;
    //    private Mock<HubCallerContext> _hubCallerContextMock;
    //    private PresenceHub? _presenceHub;

    //    [SetUp]
    //    public void Setup()
    //    {
    //        // Мок репозиторію
    //        _userRepositoryMock = new Mock<IUserRepository>();

    //        // Мок контексту Hub
    //        _hubCallerContextMock = new Mock<HubCallerContext>();
    //        _hubCallerContextMock.Setup(context => context.User)
    //            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
    //            {
    //                new Claim(ClaimTypes.NameIdentifier, "user123")
    //            })));

    //        // Створення екземпляру PresenceHub
    //        _presenceHub = new PresenceHub(_userRepositoryMock.Object)
    //        {
    //            Context = _hubCallerContextMock.Object
    //        };
    //    }

    //    [Test]
    //    public async Task OnConnectedAsync_ShouldUpdateUserPresence_WhenUserIsConnected()
    //    {
    //        // Act
    //        await _presenceHub.OnConnectedAsync();

    //        // Assert
    //        _userRepositoryMock.Verify(r => r.UpdatePresenceAsync("user123", It.IsAny<DateTime>(), true), Times.Once);
    //    }

    //    [Test]
    //    public async Task OnDisconnectedAsync_ShouldUpdateUserPresence_WhenUserIsDisconnected()
    //    {
    //        // Act
    //        await _presenceHub.OnDisconnectedAsync(null);

    //        // Assert
    //        _userRepositoryMock.Verify(r => r.UpdatePresenceAsync("user123", It.IsAny<DateTime>(), false), Times.Once);
    //    }

    //    [TearDown]
    //    public void TearDown()
    //    {
    //        // Очищення поля _presenceHub після тесту
    //        _presenceHub = null;
    //    }
    //}
}
