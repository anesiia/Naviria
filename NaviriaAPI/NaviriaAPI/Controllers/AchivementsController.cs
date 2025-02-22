using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.Services;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/achivements")]
    public class AchivementsController : ControllerBase
    {
        private readonly IAchivementService _achivementsService;

        public AchivementsController(IAchivementService achivementsService)
        {
            _achivementsService = achivementsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var achivements = await _achivementsService.GetAllAsync();
            return Ok(achivements);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var achivement = await _achivementsService.GetByIdAsync(id);
            if (achivement == null) return NotFound();
            return Ok(achivement);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] AchivementCreateDto achivementDto)
        {
            var createdAchivement = await _achivementsService.CreateAsync(achivementDto);
            return CreatedAtAction(nameof(GetById), new { id = createdAchivement.Id }, createdAchivement);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AchivementUpdateDto achivementDto)
        {
            var updated = await _achivementsService.UpdateAsync(id, achivementDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _achivementsService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
