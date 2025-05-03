using BooksAPI.Core.RequestHandler.BooksSearchHandler;
using BooksAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSearchSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookSearchController : ControllerBase
{
    private readonly BooksDbContext _context;

    public BookSearchController(BooksDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<BookSearchResponse>> SearchBooks([FromBody] BookSearchRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
            return BadRequest("Search query cannot be empty.");

        var query = request.Query.Trim().ToLower();

        var results = await _context.Books
            .Where(b =>
                b.Title.ToLower().Contains(query) ||
                b.Author.ToLower().Contains(query))
            .Select(b => new BookSearchResultsData
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author
            })
            .ToListAsync();

        var response = new BookSearchResponse
        {
            Query = request.Query,
            Results = results
        };

        return Ok(response);
    }
}
