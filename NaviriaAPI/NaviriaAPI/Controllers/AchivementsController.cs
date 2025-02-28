using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.IServices;
using NaviriaAPI.DTOs.UdateDTOs;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/achivements")]
    public class AchivementsController : ControllerBase
    {
        private readonly IAchivementService _achivementsService;
        private readonly ILogger<AchivementsController> _logger;

        public AchivementsController(IAchivementService achivementsService, ILogger<AchivementsController> logger)
        {
            _achivementsService = achivementsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var achivements = await _achivementsService.GetAllAsync();
                return Ok(achivements);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get achivements");
                return StatusCode(500, "Failed to get achivements");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var achivement = await _achivementsService.GetByIdAsync(id);
                if (achivement == null) return NotFound();
                return Ok(achivement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get achivement with ID {0}", id);
                return StatusCode(500, $"Failed to get achivement with ID {id}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AchivementCreateDto achivementDto)
        {
            try
            {
                var createdAchivement = await _achivementsService.CreateAsync(achivementDto);
                return CreatedAtAction(nameof(GetById), new { id = createdAchivement.Id }, createdAchivement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add new achivement");
                return StatusCode(500, "Failed to add new achivement");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] AchivementUpdateDto achivementDto)
        {
            try
            {
                var updated = await _achivementsService.UpdateAsync(id, achivementDto);
                return updated ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update achivement with ID {0}", id);
                return StatusCode(500, $"Failed to update achivement with ID {id}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var deleted = await _achivementsService.DeleteAsync(id);
                return deleted ? NoContent() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete achivement with ID {0}", id);
                return StatusCode(500, $"Failed to delete achivement with ID {id}");
            }
        }
    }
}
