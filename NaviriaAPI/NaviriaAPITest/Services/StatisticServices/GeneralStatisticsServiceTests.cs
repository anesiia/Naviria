using System;
using System.Threading.Tasks;
using Moq;
using NaviriaAPI.Exceptions;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services.StatisticServices;
using NaviriaAPI.Entities;
using NaviriaAPI.Constants;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services.StatisticServices
{
    [TestFixture]
    public class GeneralStatisticsServiceTests
    {
        private Mock<IStatisticRepository> _statisticRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private GeneralStatisticsService _service;

        [SetUp]
        public void SetUp()
        {
            _statisticRepoMock = new Mock<IStatisticRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _service = new GeneralStatisticsService(_statisticRepoMock.Object, _userRepoMock.Object);
        }

        [Test]
        public async Task TC001_GetTotalUserCountAsync_ReturnsCorrectValue()
        {
            _statisticRepoMock.Setup(r => r.GetTotalUserCountAsync()).ReturnsAsync(10);
            var result = await _service.GetTotalUserCountAsync();
            Assert.That(result, Is.EqualTo(10));
        }

        [Test]
        public async Task TC002_GetTotalTaskCountAsync_ReturnsCorrectValue()
        {
            _statisticRepoMock.Setup(r => r.GetTotalTaskCountAsync()).ReturnsAsync(25);
            var result = await _service.GetTotalTaskCountAsync();
            Assert.That(result, Is.EqualTo(25));
        }

        [Test]
        public async Task TC003_GetCompletedTasksPercentageAsync_WithZeroTasks_ReturnsZero()
        {
            _statisticRepoMock.Setup(r => r.GetTotalTaskCountAsync()).ReturnsAsync(0);
            var result = await _service.GetCompletedTasksPercentageAsync();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task TC004_GetCompletedTasksPercentageAsync_ReturnsCorrectPercentage()
        {
            _statisticRepoMock.Setup(r => r.GetTotalTaskCountAsync()).ReturnsAsync(10);
            _statisticRepoMock.Setup(r => r.GetCompletedTaskCountAsync()).ReturnsAsync(7);
            var result = await _service.GetCompletedTasksPercentageAsync();
            Assert.That(result, Is.EqualTo(70));
        }

        [Test]
        public void TC005_GetDaysSinceAppBirthday_ReturnsCorrectDays()
        {
            var expected = (DateTime.UtcNow.Date - ProjectConstants.AppBirthday).Days;
            var result = _service.GetDaysSinceAppBirthday();
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public async Task TC006_GetDaysSinceUserRegistrationAsync_UserNotFound_ThrowsNotFoundException()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync("nonexistent")).ReturnsAsync((UserEntity)null);

            var ex = Assert.ThrowsAsync<NotFoundException>(async () =>
                await _service.GetDaysSinceUserRegistrationAsync("nonexistent"));

            Assert.That(ex.Message, Does.Contain("User with ID nonexistent not found."));
        }

        [Test]
        public async Task TC007_GetDaysSinceUserRegistrationAsync_ReturnsCorrectDays()
        {
            var registrationDate = DateTime.UtcNow.Date.AddDays(-5);
            _userRepoMock.Setup(r => r.GetByIdAsync("user123")).ReturnsAsync(new UserEntity { RegitseredAt = registrationDate });

            var result = await _service.GetDaysSinceUserRegistrationAsync("user123");

            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public async Task TC008_GetDaysSinceUserRegistrationAsync_RegisteredToday_ReturnsZero()
        {
            var registrationDate = DateTime.UtcNow.Date;
            _userRepoMock.Setup(r => r.GetByIdAsync("user123")).ReturnsAsync(new UserEntity { RegitseredAt = registrationDate });

            var result = await _service.GetDaysSinceUserRegistrationAsync("user123");

            Assert.That(result, Is.EqualTo(0));
        }
    }
}