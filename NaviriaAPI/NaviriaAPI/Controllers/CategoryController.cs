using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Services;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing categories.
    /// Provides endpoints to create, retrieve, update, and delete categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryCleanupService _categoryCleanupService;
        private readonly ILogger<CategoryController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="categoryService">Service for category operations.</param>
        /// <param name="categoryCleanupService">Service for cascade deletion of category and its tasks.</param>
        /// <param name="logger">Logger instance.</param>
        public CategoryController(ICategoryService categoryService,
            ICategoryCleanupService categoryCleanupService,
            ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _categoryCleanupService = categoryCleanupService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await _categoryService.GetAllAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get categories");
                return StatusCode(500, "Failed to get categories");
            }
        }

        /// <summary>
        /// Gets a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>The requested category, or 404 if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var category = await _categoryService.GetByIdAsync(id);
                if (category == null) return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get category with ID {0}", id);
                return StatusCode(500, $"Failed to get category with ID {id}");
            }
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDto">The category creation DTO.</param>
        /// <returns>The created category with its ID.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateDto categoryDto)
        {
            try
            {
                var createdCategory = await _categoryService.CreateAsync(categoryDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new category");
                return StatusCode(500, "Failed to add new category");
            }
        }

        /// <summary>
        /// Updates the details of a category by its identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <param name="categoryDto">The updated category data.</param>
        /// <returns>No content if successful; 404 if category not found.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CategoryUpdateDto categoryDto)
        {
            try
            {
                var updated = await _categoryService.UpdateAsync(id, categoryDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update category with ID {0}", id);
                return StatusCode(500, $"Failed to update category with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a category and all its related tasks by category identifier.
        /// </summary>
        /// <param name="id">The category identifier.</param>
        /// <returns>No content if successful; 404 if category not found.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _categoryCleanupService.DeleteCategoryAndTasksAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete category with ID {0}", id);
                return StatusCode(500, $"Failed to delete category with ID {id}");
            }
        }
    }
}
