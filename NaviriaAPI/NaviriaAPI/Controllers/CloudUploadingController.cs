using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.IServices.ICloudStorage;
using NaviriaAPI.IServices;
using NaviriaAPI.Helpers;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for uploading images to cloud storage (Cloudinary).
    /// Provides an endpoint for uploading images and retrieving the cloud URL.
    /// </summary>
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

        /// <summary>
        /// Uploads an image file to cloud storage (Cloudinary) and returns its URL.
        /// </summary>
        /// <param name="file">The image file to upload.</param>
        /// <returns>
        /// 200: The URL of the uploaded image.<br/>
        /// 400: If no file was uploaded.<br/>
        /// 500: If an internal error occurs during upload.
        /// </returns>
        /// 
        [Authorize(Roles = Roles.User)]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            try
            {
                var url = await _cloudinaryService.UploadImageAndGetUrlAsync(file);
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload image to cloud storage");
                return StatusCode(500, "Failed to upload image");
            }
        }
    }
}
