using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UdateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var friendsRequests = await _friendRequestService.GetAllAsync();
            return Ok(friendsRequests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var FriendRequest = await _friendRequestService.GetByIdAsync(id);
            if (FriendRequest == null) return NotFound();
            return Ok(FriendRequest);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] FriendRequestCreateDto FriendRequestDto)
        {
            var createdFriendRequest = await _friendRequestService.CreateAsync(FriendRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = createdFriendRequest.Id }, createdFriendRequest);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] FriendRequestUpdateDto FriendRequestDto)
        {
            var updated = await _friendRequestService.UpdateAsync(id, FriendRequestDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _friendRequestService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
