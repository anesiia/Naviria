using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.Services.User;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly ILogger<FolderController> _logger;

        public FolderController(IFolderService folderService, ILogger<FolderController> logger)
        {
            _folderService = folderService;
            _logger = logger;
        }


        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAll(string id)
        {
            try
            {
                var qoutes = await _folderService.GetAllByUserIdAsync(id);
                return Ok(qoutes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get folders for user with ID {0}", id);
                return StatusCode(500, "Failed to get folders for user with ID {id}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Folder ID is required.");

            try
            {
                var User = await _folderService.GetByIdAsync(id);
                if (User == null) return NotFound();
                return Ok(User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get folder with ID {0}", id);
                return StatusCode(500, $"Failed to get folder with ID {id}");
            }

        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FolderCreateDto folderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var folder = await _folderService.CreateAsync(folderDto);
                return Ok(new { folder });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create folder");
                return StatusCode(500, "Failed to create folder");
            }
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FolderUpdateDto folderDto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Folder ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _folderService.UpdateAsync(id, folderDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update folder with ID {0}", id);
                return StatusCode(500, $"Failed to get update folder with ID {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Folder ID is required.");

            try
            {
                var deleted = await _folderService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete folder with ID {0}", id);
                return StatusCode(500, $"Failed to delete folder with ID {id}");
            }
        }
    }
}