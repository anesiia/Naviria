using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CloudUploadingController : ControllerBase
    {
        private readonly ILogger<CloudUploadingController> _logger;
        public readonly ICloudinaryService _cloudinaryService;

        public CloudUploadingController(
            ILogger<CloudUploadingController> logger,
            ICloudinaryService cloudinaryService)
        {
            _logger = logger;
            _cloudinaryService = cloudinaryService;
        }

        [AllowAnonymous]
        [HttpPost("upload-profile-photo")]
        public async Task<IActionResult> UploadProfilePhoto([FromForm] string userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                var answer = await _cloudinaryService.UploadImageAsync(userId, file);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload user photo");
                return StatusCode(500, $"Failed to upload user photo");
            }
        }
    }
}
