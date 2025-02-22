using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.CreateDTOs;
using NaviriaAPI.DTOs.UdateDTOs;
using NaviriaAPI.DTOs.UpdateDTOs;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/quotes")]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var qoutes = await _quoteService.GetAllAsync();
            return Ok(qoutes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var quote = await _quoteService.GetByIdAsync(id);
            if (quote == null) return NotFound();
            return Ok(quote);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Create([FromBody] QuoteCreateDto quoteDto)
        {
            var createdQuote = await _quoteService.CreateAsync(quoteDto);
            return CreatedAtAction(nameof(GetById), new { id = createdQuote.Id }, createdQuote);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] QuoteUpdateDto quoteDto)
        {
            var updated = await _quoteService.UpdateAsync(id, quoteDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _quoteService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
