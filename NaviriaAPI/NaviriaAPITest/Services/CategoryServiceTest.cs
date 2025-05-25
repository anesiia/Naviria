using Moq;
using NaviriaAPI.DTOs; 
using NaviriaAPI.DTOs.Category;
using NaviriaAPI.Entities;
using NaviriaAPI.IRepositories;
using NaviriaAPI.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaviriaAPI.Tests.Services
{
    [TestFixture]
    public class CategoryServiceTest
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(_mockCategoryRepository.Object);
        }

        
        [Test]
        public async Task TC01_CreateCategory_WithValidData_ReturnsCategoryDto()
        {
            // Arrange
            var createDto = new CategoryCreateDto { Name = "Science" };

            var categoryEntity = new CategoryEntity
            {
                Id = "1", 
                Name = createDto.Name
            };

            
            _mockCategoryRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<CategoryEntity>())) 
                .Callback<CategoryEntity>(entity => entity.Id = "1") 
                .Returns(Task.CompletedTask); 

            // Act
            var result = await _categoryService.CreateAsync(createDto);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.IsAny<CategoryEntity>()), Times.Once); 
            Assert.That(result, Is.Not.Null); 
            Assert.That(result.Name, Is.EqualTo("Science")); 
            Assert.That(result.Id, Is.EqualTo("1")); 
        }

      
        [Test]
        public async Task TC02_UpdateCategory_WithValidData_ReturnsTrue()
        {
            // Arrange
            var updateDto = new CategoryUpdateDto { Name = "Technology" };

            _mockCategoryRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<CategoryEntity>()))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.UpdateAsync("1", updateDto);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<CategoryEntity>()), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC03_GetCategory_ByValidId_ReturnsCategoryDto()
        {
            // Arrange
            var category = new CategoryDto { Id = "1", Name = "Science" };

            _mockCategoryRepository
                .Setup(repo => repo.GetByIdAsync("1"))
                .ReturnsAsync(new CategoryEntity { Id = "1", Name = "Science" }); 

            // Act
            var result = await _categoryService.GetByIdAsync("1");

            // Assert
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync("1"), Times.Once);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Science"));
        }

        [Test]
        public async Task TC04_GetCategory_ByInvalidId_ReturnsNull()
        {
            // Arrange
            _mockCategoryRepository
                .Setup(repo => repo.GetByIdAsync("invalid-id"))
                .ReturnsAsync((CategoryEntity)null);

            // Act
            var result = await _categoryService.GetByIdAsync("invalid-id");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task TC05_DeleteCategory_ByValidId_ReturnsTrue()
        {
            // Arrange
            _mockCategoryRepository
                .Setup(repo => repo.DeleteAsync("1"))
                .ReturnsAsync(true);

            // Act
            var result = await _categoryService.DeleteAsync("1");

            // Assert
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TC06_DeleteCategory_ByInvalidId_ReturnsFalse()
        {
            // Arrange
            _mockCategoryRepository
                .Setup(repo => repo.DeleteAsync("invalid-id"))
                .ReturnsAsync(false);

            // Act
            var result = await _categoryService.DeleteAsync("invalid-id");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task TC07_GetAllCategories_ReturnsCategoryList()
        {
            // Arrange
            var categories = new List<CategoryEntity>
            {
                new CategoryEntity { Id = "1", Name = "Science" },
                new CategoryEntity { Id = "2", Name = "Technology" },
                new CategoryEntity { Id = "3", Name = "Literature" }
            };

            _mockCategoryRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync();

            // Assert
            _mockCategoryRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
            Assert.That(result.Count(), Is.EqualTo(3));
        }
    }
}
