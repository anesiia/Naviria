using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.FriendRequest;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing friend requests.
    /// Provides endpoints to create, retrieve, update, and delete friend requests.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly ILogger<FriendRequestController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendRequestController"/> class.
        /// </summary>
        /// <param name="friendRequestService">Service for friend request operations.</param>
        /// <param name="logger">Logger instance.</param>
        public FriendRequestController(IFriendRequestService friendRequestService, ILogger<FriendRequestController> logger)
        {
            _friendRequestService = friendRequestService;
            _logger = logger;
        }

        /// <summary>
        /// Gets a list of all friend requests in the system.
        /// </summary>
        /// <returns>
        /// 200: Returns a list of friend requests.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var friendsRequests = await _friendRequestService.GetAllAsync();
                return Ok(friendsRequests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get friends requests");
                return StatusCode(500, "Failed to get friends requests");
            }
        }

        /// <summary>
        /// Gets a friend request by its identifier.
        /// </summary>
        /// <param name="id">The friend request identifier.</param>
        /// <returns>
        /// 200: Returns the friend request.<br/>
        /// 404: If the friend request is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var friendRequest = await _friendRequestService.GetByIdAsync(id);
                if (friendRequest == null) return NotFound();
                return Ok(friendRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get friend request with ID {0}", id);
                return StatusCode(500, $"Failed to get friend request with ID {id}");
            }
        }

        /// <summary>
        /// Creates a new friend request.
        /// </summary>
        /// <param name="friendRequestDto">The friend request creation DTO.</param>
        /// <returns>
        /// 201: The created friend request with its ID.<br/>
        /// 400: If input data is invalid.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FriendRequestCreateDto friendRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdFriendRequest = await _friendRequestService.CreateAsync(friendRequestDto);
                return CreatedAtAction(nameof(GetById), new { id = createdFriendRequest.Id }, createdFriendRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new friend request");
                return StatusCode(500, "Failed to add new friend request");
            }
        }

        /// <summary>
        /// Updates a friend request by its identifier.
        /// </summary>
        /// <param name="id">The friend request identifier.</param>
        /// <param name="friendRequestDto">The updated friend request data.</param>
        /// <returns>
        /// 204: If the update was successful.<br/>
        /// 400: If the model state is invalid.<br/>
        /// 404: If the friend request is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FriendRequestUpdateDto friendRequestDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _friendRequestService.UpdateAsync(id, friendRequestDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update friend request with ID {0}", id);
                return StatusCode(500, $"Failed to update friend request with ID {id}");
            }
        }

        /// <summary>
        /// Deletes a friend request by its identifier.
        /// </summary>
        /// <param name="id">The friend request identifier.</param>
        /// <returns>
        /// 204: If the deletion was successful.<br/>
        /// 404: If the friend request is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _friendRequestService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete friend request with ID {0}", id);
                return StatusCode(500, $"Failed to delete friend request with ID {id}");
            }
        }

        /// <summary>
        /// Gets all incoming friend requests for a specific user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>
        /// 200: Returns a list of incoming friend requests.<br/>
        /// 400: If the user ID is missing.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet("incoming/{userId}")]
        public async Task<IActionResult> GetIncomingRequests(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest("User ID is required.");

            try
            {
                var requests = await _friendRequestService.GetIncomingRequestsAsync(userId);
                return Ok(requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get incoming friend requests for user {UserId}", userId);
                return StatusCode(500, $"Failed to get incoming friend requests for user {userId}");
            }
        }
    }
}
