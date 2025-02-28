using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Services;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<AchievementsController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<AchievementsController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _categoryService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound(); ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete category with ID {0}", id);
                return StatusCode(500, $"Failed to delete category with ID {id}");
            }
        }
    }
}
