using BooksAPI.Core.RequestHandler.BooksSearchHandler;
using BooksAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookSearchSolution.API.Controllers;

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
    public async Task<ActionResult<BookSearchResponse>> SearchBooks([FromBody] BookSearchRequest request)
    {
        _logger.LogInformation("SearchBooks API called with parameters: {@Request}", request);

        try
        {
            var result = await _booksSearchHandler.SearchBooks(request);

            if (result == null)
            {
                _logger.LogWarning("No books found for search query: {@Request}", request);
                return NotFound("No books found matching your search query.");
            }

            _logger.LogInformation("Books found: {@Result}", result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching for books with query: {@Request}", request);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }
}
