using Moq;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.Folder;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services;
using NUnit.Framework;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class FolderServiceTests
    {
        private Mock<IFolderRepository> _folderRepoMock = null!;
        private FolderService _folderService = null!;

        [SetUp]
        public void SetUp()
        {
            _folderRepoMock = new Mock<IFolderRepository>();
            _folderService = new FolderService(_folderRepoMock.Object);
        }

        [Test]
        public async Task TC001_GetAllByUserIdAsync_ReturnsMappedFolders()
        {
            // Arrange
            var userId = "user123";
            var folders = new List<FolderEntity>
            {
                new FolderEntity { Id = "1", UserId = userId, Name = "Work" },
                new FolderEntity { Id = "2", UserId = userId, Name = "Study" }
            };
            _folderRepoMock.Setup(r => r.GetAllByUserIdAsync(userId)).ReturnsAsync(folders);

            // Act
            var result = (await _folderService.GetAllByUserIdAsync(userId)).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Work"));
            Assert.That(result[1].Name, Is.EqualTo("Study"));
        }

        [Test]
        public async Task TC002_GetByIdAsync_ValidId_ReturnsMappedFolder()
        {
            var folder = new FolderEntity { Id = "123", UserId = "u1", Name = "Folder1" };
            _folderRepoMock.Setup(r => r.GetByIdAsync("123")).ReturnsAsync(folder);

            var result = await _folderService.GetByIdAsync("123");

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo("123"));
            Assert.That(result.Name, Is.EqualTo("Folder1"));
        }

        [Test]
        public async Task TC003_GetByIdAsync_InvalidId_ReturnsNull()
        {
            _folderRepoMock.Setup(r => r.GetByIdAsync("999")).ReturnsAsync((FolderEntity?)null);

            var result = await _folderService.GetByIdAsync("999");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC004_CreateAsync_ValidDto_CreatesAndReturnsDto()
        {
            var createDto = new FolderCreateDto
            {
                UserId = "user1",
                Name = "New Folder"
            };

            FolderEntity? createdEntity = null;
            _folderRepoMock
                .Setup(r => r.CreateAsync(It.IsAny<FolderEntity>()))
                .Callback<FolderEntity>(e => createdEntity = e)
                .Returns(Task.CompletedTask);

            var result = await _folderService.CreateAsync(createDto);

            Assert.That(result.UserId, Is.EqualTo("user1"));
            Assert.That(result.Name, Is.EqualTo("New Folder"));
            Assert.That(createdEntity, Is.Not.Null);
            Assert.That(createdEntity!.Name, Is.EqualTo("New Folder"));
        }

        [Test]
        public async Task TC005_UpdateAsync_ExistingFolder_UpdatesAndReturnsTrue()
        {
            var id = "abc123";
            var entity = new FolderEntity { Id = id, Name = "Old Name", UserId = "user1" };
            var updateDto = new FolderUpdateDto { Name = "Updated Name" };

            _folderRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
            _folderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<FolderEntity>())).ReturnsAsync(true);

            var result = await _folderService.UpdateAsync(id, updateDto);

            Assert.That(result, Is.True);
            Assert.That(entity.Name, Is.EqualTo("Updated Name"));
        }

        [Test]
        public async Task TC006_UpdateAsync_NonExistentFolder_ReturnsFalse()
        {
            var id = "invalid123";
            var updateDto = new FolderUpdateDto { Name = "Try Update" };

            _folderRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((FolderEntity?)null);

            var result = await _folderService.UpdateAsync(id, updateDto);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC007_DeleteAsync_ValidId_ReturnsTrue()
        {
            _folderRepoMock.Setup(r => r.DeleteAsync("id123")).ReturnsAsync(true);

            var result = await _folderService.DeleteAsync("id123");

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC008_DeleteAsync_InvalidId_ReturnsFalse()
        {
            _folderRepoMock.Setup(r => r.DeleteAsync("invalid")).ReturnsAsync(false);

            var result = await _folderService.DeleteAsync("invalid");

            Assert.That(result, Is.False);
        }
    }
}
