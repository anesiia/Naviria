using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using NaviriaAPI.Services.CleanupServices;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Entities;
using NaviriaAPI.Exceptions;

namespace NaviriaAPI.Tests.Services.CleanupServices
{
    [TestFixture]
    public class CategoryCleanupServiceTests
    {
        private Mock<ICategoryRepository> _categoryRepoMock = null!;
        private Mock<ITaskRepository> _taskRepoMock = null!;
        private CategoryCleanupService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _service = new CategoryCleanupService(_categoryRepoMock.Object, _taskRepoMock.Object);
        }

        [Test]
        public void TC001_DeleteCategoryAndTasksAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
        {
            // Arrange
            var categoryId = "123";
            _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync((CategoryEntity?)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() =>
                _service.DeleteCategoryAndTasksAsync(categoryId));

            Assert.That(ex!.Message, Is.EqualTo("Category with ID 123 not found."));
        }

        [Test]
        public async Task TC002_DeleteCategoryAndTasksAsync_ShouldReturnTrue_WhenCategoryDeletedSuccessfully()
        {
            // Arrange
            var categoryId = "456";
            var category = new CategoryEntity { Id = categoryId, Name = "Test Category" };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _taskRepoMock.Setup(r => r.DeleteManyByCategoryIdAsync(categoryId))
                .Returns(Task.CompletedTask);

            _categoryRepoMock.Setup(r => r.DeleteAsync(categoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteCategoryAndTasksAsync(categoryId);

            // Assert
            Assert.That(result, Is.True);
            _taskRepoMock.Verify(r => r.DeleteManyByCategoryIdAsync(categoryId), Times.Once);
            _categoryRepoMock.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }

        [Test]
        public async Task TC003_DeleteCategoryAndTasksAsync_ShouldReturnFalse_WhenCategoryDeletionFails()
        {
            // Arrange
            var categoryId = "789";
            var category = new CategoryEntity { Id = categoryId, Name = "Failing Category" };

            _categoryRepoMock.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            _taskRepoMock.Setup(r => r.DeleteManyByCategoryIdAsync(categoryId))
                .Returns(Task.CompletedTask);

            _categoryRepoMock.Setup(r => r.DeleteAsync(categoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DeleteCategoryAndTasksAsync(categoryId);

            // Assert
            Assert.That(result, Is.False);
            _taskRepoMock.Verify(r => r.DeleteManyByCategoryIdAsync(categoryId), Times.Once);
            _categoryRepoMock.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }
    }
}