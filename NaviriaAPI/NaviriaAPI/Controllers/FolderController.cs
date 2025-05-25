using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Folder;
using NaviriaAPI.IServices;
using NaviriaAPI.IServices.ICleanupServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing user folders.
    /// Provides endpoints to create, retrieve, update, and delete folders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IFolderCleanupService _folderCleanupService;
        private readonly ILogger<FolderController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderController"/> class.
        /// </summary>
        /// <param name="folderService">Service for folder operations.</param>
        /// <param name="folderCleanupService">Service for cascading deletion of folders and their tasks.</param>
        /// <param name="logger">Logger instance.</param>
        public FolderController(IFolderService folderService,
            IFolderCleanupService folderCleanupService,
            ILogger<FolderController> logger)
        {
            _folderService = folderService;
            _folderCleanupService = folderCleanupService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all folders for the specified user.
        /// </summary>
        /// <param name="id">The user ID.</param>
        /// <returns>
        /// 200: Returns a list of folders.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAll(string id)
        {
            try
            {
                var folders = await _folderService.GetAllByUserIdAsync(id);
                return Ok(folders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get folders for user with ID {0}", id);
                return StatusCode(500, $"Failed to get folders for user with ID {id}");
            }
        }

        /// <summary>
        /// Retrieves a folder by its identifier.
        /// </summary>
        /// <param name="id">The folder ID.</param>
        /// <returns>
        /// 200: The requested folder.<br/>
        /// 400: If the folder ID is not provided.<br/>
        /// 404: If the folder is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Folder ID is required.");

            try
            {
                var folder = await _folderService.GetByIdAsync(id);
                if (folder == null) return NotFound();
                return Ok(folder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get folder with ID {0}", id);
                return StatusCode(500, $"Failed to get folder with ID {id}");
            }
        }

        /// <summary>
        /// Creates a new folder.
        /// </summary>
        /// <param name="folderDto">The folder creation DTO.</param>
        /// <returns>
        /// 200: The created folder.<br/>
        /// 400: If the request body is invalid.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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

        /// <summary>
        /// Updates the details of a folder by its identifier.
        /// </summary>
        /// <param name="id">The folder ID.</param>
        /// <param name="folderDto">The updated folder data.</param>
        /// <returns>
        /// 204: If the update was successful.<br/>
        /// 400: If the folder ID is missing or model state is invalid.<br/>
        /// 404: If the folder is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
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
                return StatusCode(500, $"Failed to update folder with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a folder and all tasks within it by the folder identifier.
        /// </summary>
        /// <param name="id">The folder ID.</param>
        /// <returns>
        /// 204: If the deletion was successful.<br/>
        /// 400: If the folder ID is not provided.<br/>
        /// 404: If the folder is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Folder ID is required.");

            try
            {
                var deleted = await _folderCleanupService.DeleteFolderAndTasksAsync(id);
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