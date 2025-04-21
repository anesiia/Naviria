using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        private readonly ILogger<FriendRequestController> _logger;

        public FriendRequestController(IFriendRequestService friendRequestService, ILogger<FriendRequestController> logger)
        {
            _friendRequestService = friendRequestService;
            _logger = logger;
        }

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
