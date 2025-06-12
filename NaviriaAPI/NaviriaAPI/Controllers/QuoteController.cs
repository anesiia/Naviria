using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviriaAPI.DTOs.Quote;
using NaviriaAPI.IServices;

namespace NaviriaAPI.Controllers
{
    /// <summary>
    /// API controller for managing motivational quotes.
    /// Provides endpoints to create, retrieve, update, and delete quotes.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteController"/> class.
        /// </summary>
        /// <param name="quoteService">Service for quote operations.</param>
        public QuoteController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        /// <summary>
        /// Gets the list of all quotes.
        /// </summary>
        /// <returns>
        /// 200: Returns a list of all quotes.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quotes = await _quoteService.GetAllAsync();
            return Ok(quotes);
        }

        /// <summary>
        /// Gets a quote by its identifier.
        /// </summary>
        /// <param name="id">The quote identifier.</param>
        /// <returns>
        /// 200: Returns the quote.<br/>
        /// 404: If the quote is not found.
        /// </returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var quote = await _quoteService.GetByIdAsync(id);
            if (quote == null) return NotFound();
            return Ok(quote);
        }

        /// <summary>
        /// Creates a new quote.
        /// </summary>
        /// <param name="quoteDto">The quote creation DTO.</param>
        /// <returns>
        /// 201: The created quote with its ID.<br/>
        /// 400: If the input data is invalid.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuoteCreateDto quoteDto)
        {
            var createdQuote = await _quoteService.CreateAsync(quoteDto);
            return CreatedAtAction(nameof(GetById), new { id = createdQuote.Id }, createdQuote);
        }

        /// <summary>
        /// Updates the details of a quote by its identifier.
        /// </summary>
        /// <param name="id">The quote identifier.</param>
        /// <param name="quoteDto">The updated quote data.</param>
        /// <returns>
        /// 204: If the update was successful.<br/>
        /// 404: If the quote is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] QuoteUpdateDto quoteDto)
        {
            var updated = await _quoteService.UpdateAsync(id, quoteDto);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Deletes a quote by its identifier.
        /// </summary>
        /// <param name="id">The quote identifier.</param>
        /// <returns>
        /// 204: If the deletion was successful.<br/>
        /// 404: If the quote is not found.<br/>
        /// 500: If an internal error occurs.
        /// </returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _quoteService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
