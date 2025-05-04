using System;
using System.Linq;
using System.Threading.Tasks;
using BooksAPI.Core.RequestHandler.SortStrategies;
using BooksAPI.Infrastructure;
using BooksAPI.Infrastructure.BooksDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler
{
    public class BookSearchHandler
    {
        private readonly BooksDbContext _context;
        private readonly ILogger<BookSearchHandler> _logger; 

        public BookSearchHandler(BooksDbContext context, ILogger<BookSearchHandler> logger)
        {
            _context = context;
            _logger = logger;  
        }

        public async Task<BookSearchResponse> SearchBooks(BookSearchRequest request)
        {
            _logger.LogInformation("SearchBooks API called with parameters: {@Request}", request);

            IQueryable<Books> query = _context.Books;

            try
            {
                if (request != null)
                {
                    if (request.Id != 0 && request.Id != null)
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

                    if (request.Pages != 0 && request.Pages != null)
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

                _logger.LogInformation("Search found {ResultsCount} books matching the query.", results.Count);

                var searchHistory = new SearchHistory
                {
                    UserId = 12345,  
                    SearchTerm = request.Query,
                    Title = request.Title,
                    Author = request.Author,
                    Genre = request.Genre,
                    Description = request.Description,
                    PublishedDate = request.PublishedDate,
                    Pages = request.Pages,
                    MinPages = request.MinPages,
                    MaxPages = request.MaxPages,
                    PublishedFrom = request.PublishedFrom,
                    PublishedTo = request.PublishedTo,
                    SortBy = request.SortBy,
                    SearchDate = DateTime.UtcNow 
                };

                _logger.LogInformation("Saving search history for query: {@SearchHistory}", searchHistory);

                await _context.SearchHistory.AddAsync(searchHistory);
                await _context.SaveChangesAsync();

                return new BookSearchResponse
                {
                    Results = results
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for books with query: {@Request}", request);
                throw;  
            }
        }
    }
}
