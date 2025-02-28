using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var qoutes = await _userService.GetAllAsync();
            return Ok(qoutes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var User = await _userService.GetByIdAsync(id);
            if (User == null) return NotFound();
            return Ok(User);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] UserCreateDto UserDto)
        {
            var createdUser = await _userService.CreateAsync(UserDto);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UserUpdateDto UserDto)
        {
            var updated = await _userService.UpdateAsync(id, UserDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _userService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
