using BooksAPI.Core.RequestHandler.BooksSearchHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BookSearchSolution.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookSearchController : ControllerBase
    {
        private readonly BookSearchHandler _booksSearchHandler;
        private readonly ILogger<BookSearchController> _logger;

        public BookSearchController(BookSearchHandler booksSearchHandler, ILogger<BookSearchController> logger)
        {
            _booksSearchHandler = booksSearchHandler;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BookSearchResponse>> SearchBooks([FromBody] BookSearchRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("SearchBooks called with null request body.");
                return BadRequest(new
                {
                    Message = "Search request cannot be empty.",
                    Help = "Ensure your request contains valid search parameters."
                });
            }

            try
            {
                _logger.LogInformation("SearchBooks API initiated. Parameters: {@Request}", request);

                var result = await _booksSearchHandler.SearchBooks(request);

                if (result?.Results == null || result.Results.Count == 0)
                {
                    _logger.LogInformation("No books found for query: {@Request}", request);
                    return NotFound(new
                    {
                        Message = "No books found matching your search criteria.",
                        Help = "Try using broader search terms or remove filters to improve your results."
                    });
                }

                _logger.LogInformation("SearchBooks successful. Found {Count} results.", result.Results.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred while processing book search. Request: {@Request}", request);

                return StatusCode(500, new
                {
                    Message = "Something went wrong while searching for books.",
                    Help = "Please try again later. If the problem persists, contact support and provide this time: " + DateTime.UtcNow
                });
            }
        }
    }
}
