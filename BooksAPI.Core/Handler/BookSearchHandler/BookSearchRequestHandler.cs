using Microsoft.EntityFrameworkCore;

using BooksAPI.Core.RequestHandler.BooksSearchHandler;
using BooksAPI.Infrastructure;
using BooksAPI.Infrastructure.BooksDB.Entities;
using BooksAPI.Core.RequestHandler.SortStrategies;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler;

public class BookSearchHandler
{
    private readonly BooksDbContext _context;

    public BookSearchHandler(BooksDbContext context)
    {
        _context = context;
    }

    public async Task<BookSearchResponse> SearchBooks(BookSearchRequest request)
    {
       
        IQueryable<Books> query = _context.Books;

        if (request != null)
        {
            if (request.Id != 0 && request.Id !=null)
                query = query.Where(b => b.Id == request.Id);

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                query = query.Where(b => b.Title.ToLower().Contains(request.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                query = query.Where(b => b.Author.ToLower().Contains(request.Author.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Genre))
            {
                query = query.Where(b => b.Genre.ToLower().Contains(request.Genre.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                query = query.Where(b => b.Description.ToLower().Contains(request.Description.ToLower()));
            }

            if (request.PublishedDate != null)
                query = query.Where(b => b.PublishedDate == request.PublishedDate);

            if (request.Pages !=0 && request.Pages != null)
                query = query.Where(b => b.Pages == request.Pages);

            if (request.PublishedFrom.HasValue)
                query = query.Where(b => b.PublishedDate >= request.PublishedFrom.Value);

            if (request.PublishedTo.HasValue)
                query = query.Where(b => b.PublishedDate <= request.PublishedTo.Value);

            if (request.MinPages.HasValue)
                query = query.Where(b => b.Pages >= request.MinPages.Value);

            if (request.MaxPages.HasValue)
                query = query.Where(b => b.Pages <= request.MaxPages.Value);


            var sorter = BookSortStrategyFactory.GetStrategy(request.SortBy);
            query = sorter.ApplySort(query, request.Query);




        }

        var results = await query
            .Select(b => new BookSearchResultsData
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                Description = b.Description,
                PublishedDate = Convert.ToDateTime(b.PublishedDate).Date,
                Pages = b.Pages

                
            })
            .ToListAsync();

        return new BookSearchResponse
        {
            Results = results
        };
    }
}