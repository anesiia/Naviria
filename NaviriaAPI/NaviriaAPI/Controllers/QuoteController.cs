using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Quote;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuoteController(IQuoteService quoteService)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuoteCreateDto quoteDto)
        {
            var createdQuote = await _quoteService.CreateAsync(quoteDto);
            return CreatedAtAction(nameof(GetById), new { id = createdQuote.Id }, createdQuote);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] QuoteUpdateDto quoteDto)
        {
            var updated = await _quoteService.UpdateAsync(id, quoteDto);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _quoteService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
