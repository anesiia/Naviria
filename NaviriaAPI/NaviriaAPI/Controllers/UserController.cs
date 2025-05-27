using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using NaviriaAPI.IServices.IEmbeddedServices;
using NaviriaAPI.IServices.IUserServices;
using NaviriaAPI.IServices.ICleanupServices;
using NaviriaAPI.DTOs.User;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// Controller for managing users data.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserCleanupService _userCleanupService;
        private readonly ISupportService _supportService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            IUserCleanupService userCleanupService,
            ISupportService supportService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _userCleanupService = userCleanupService;
            _supportService = supportService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all users in the system.
        /// </summary>
        /// <returns>List of all users.</returns>
        /// <response code="200">Returns a list of all users.</response>
        /// <response code="500">If an error occurs while getting users.</response>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all users");
                return StatusCode(500, "Failed to get all users");
            }
        }

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>User with the specified ID.</returns>
        /// <response code="200">Returns the user.</response>
        /// <response code="404">If user is not found.</response>
        /// <response code="500">If an error occurs while getting the user.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null) return NotFound();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user by id");
                return StatusCode(500, "Failed to get user by id");
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="UserDto">The user creation data.</param>
        /// <returns>The JWT token of the created user.</returns>
        /// <response code="200">Returns the created user's token.</response>
        /// <response code="400">If the model state is invalid.</response>
        /// <response code="409">If a user with the given email already exists.</response>
        /// <response code="422">If a user with the given nickname already exists.</response>
        /// <response code="500">If an unexpected error occurs while creating the user.</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto UserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _userService.CreateAsync(UserDto);
                return Ok(new { token });
            }
            catch (EmailAlreadyExistException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return StatusCode(409, ex.Message);
            }
            catch (NicknameAlreadyExistException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return StatusCode(422, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user");
                return StatusCode(500, "Failed to create user");
            }
        }


        /// <summary>
        /// Updates user information by user ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <param name="UserDto">The user update data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the update was successful.</response>
        /// <response code="400">If the model state is invalid or ID is missing.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an error occurs while updating the user.</response>
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto UserDto)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _userService.UpdateAsync(id, UserDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update user with ID {0}", id);
                return StatusCode(500, $"Failed to update user with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a user and all related data by user ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>No content if deleted.</returns>
        /// <response code="204">If the user was deleted.</response>
        /// <response code="400">If the ID is missing.</response>
        /// <response code="404">If the user is not found.</response>
        /// <response code="500">If an error occurs while deleting the user.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");

            try
            {
                var deleted = await _userCleanupService.DeleteUserAndRelatedDataAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete user with ID {0}", id);
                return StatusCode(500, $"Failed to delete user with ID {id}");
            }
        }

        /// <summary>
        /// Gives an achievement to a user.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <param name="achievementId">Achievement ID.</param>
        /// <returns>No content if successful.</returns>
        /// <response code="204">If the achievement was given.</response>
        /// <response code="400">If IDs are missing.</response>
        /// <response code="404">If the user or achievement is not found.</response>
        /// <response code="500">If an error occurs while giving the achievement.</response>
        [HttpPut("{userId}/give-achievement/{achievementId}")]
        public async Task<IActionResult> GiveAchievementToUser(string userId, string achievementId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(achievementId))
                return BadRequest("User ID and Achievement ID are required.");

            try
            {
                var updated = await _userService.GiveAchievementAsync(userId, achievementId);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to give achievement to user with ID {0}", userId);
                return StatusCode(500, $"Failed to give achievement to user with ID {userId}");
            }
        }

        /// <summary>
        /// Sends a support message from one user to another.
        /// </summary>
        /// <param name="senderId">Sender user ID.</param>
        /// <param name="receiverId">Receiver user ID.</param>
        /// <returns>"Support sent" if successful.</returns>
        /// <response code="200">If support message sent.</response>
        /// <response code="400">If IDs are missing or invalid.</response>
        /// <response code="500">If an error occurs while sending support.</response>
        [HttpPost("support/from-{senderId}/to-{receiverId}")]
        public async Task<IActionResult> SendSupport(string senderId, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(senderId) && string.IsNullOrWhiteSpace(receiverId))
                return BadRequest("User ID is required.");

            try
            {
                var user = await _userService.GetByIdAsync(senderId);
                if (user == null)
                    throw new NotFoundException("Failed to get user by id");

                await _supportService.SendSupportAsync(senderId, receiverId);
                return Ok("Support sent");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Uploads a new profile photo for the user.
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="file">Profile image file.</param>
        /// <returns>Upload result (success, url, achievement granted).</returns>
        /// <response code="200">If upload is successful.</response>
        /// <response code="400">If the request is invalid or missing data.</response>
        /// <response code="404">If user is not found.</response>
        /// <response code="500">If an error occurs during upload.</response>
        [HttpPost("{id}/upload-profile-photo")]
        public async Task<IActionResult> UploadProfilePhoto(string id, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("User ID is required.");
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded.");

            try
            {
                var result = await _userService.UploadUserProfilePhotoAsync(id, file);

                return result ? Ok(new { success = true }) : StatusCode(500, "Failed to upload photo.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload profile photo for user {0}", id);
                return StatusCode(500, "Failed to upload profile photo.");
            }
        }

    }
}
