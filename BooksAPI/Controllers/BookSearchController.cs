using BooksAPI.Core.RequestHandler.BooksSearchHandler;
using BooksAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSearchSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookSearchController : ControllerBase
{
    private readonly BookSearchHandler _booksSearchHandler;

    public BookSearchController(BookSearchHandler booksSearchHandler)
    {
        _booksSearchHandler = booksSearchHandler;
    }
    

    [HttpPost]
    public async Task<ActionResult<BookSearchResponse>> SearchBooks([FromBody] BookSearchRequest request)
    {

        var result = await _booksSearchHandler.SearchBooks(request);

        if (result == null)
        {
            return NotFound("No books found matching your search query.");
        }

        return Ok(result);
    }
}
