using System;
using System.Linq;
using System.Threading.Tasks;
using BooksAPI.Core.Handler.BookSearchHandler;
using BooksAPI.Core.RequestHandler.SortStrategies;
using BooksAPI.Infrastructure;
using BooksAPI.Infrastructure.BooksDB.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler
{
    public class BookSearchHandler
    {
        private readonly BooksDbContext _context;
        private readonly GenericFilterService<Books> _filterService;
        private readonly BookSearchService _searchService;
        private readonly IBookSortStrategy _sortStrategy;
        private readonly ILogger<BookSearchHandler> _logger;

        public BookSearchHandler(
            BooksDbContext context,
            GenericFilterService<Books> filterService,
            BookSearchService searchService,
            IBookSortStrategy sortStrategy,
            ILogger<BookSearchHandler> logger)
        {
            _context = context;
            _filterService = filterService;
            _searchService = searchService;
            _sortStrategy = sortStrategy;
            _logger = logger;
        }

        public async Task<BookSearchResponse> SearchBooks(BookSearchRequest request)
        {
            if (request == null)
            {
                _logger.LogWarning("Book search attempted with null request.");
                throw new ArgumentNullException(nameof(request), "Search request cannot be null.");
            }

            try
            {
                _logger.LogInformation("Book search initiated. Parameters: {@Request}", request);

                var query = _context.Books.AsQueryable();

                try
                {
                    query = _filterService.ApplyFilters(query, request.Filters);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to apply filters for request: {@Request}", request);
                    throw new InvalidOperationException("There was a problem applying search filters. Please check your filter values.");
                }

                try
                {
                    query = _searchService.ApplySearch(query, request.Query);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to apply keyword search: {Keyword}", request.Query);
                    throw new InvalidOperationException("There was a problem with your keyword search. Try simplifying your query.");
                }

                try
                {
                    query = _sortStrategy.ApplySort(query, request.SortField, request.SortOrder, request.Query);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sorting failed for field '{Field}' and order '{Order}'", request.SortField, request.SortOrder);
                    throw new InvalidOperationException("An error occurred while sorting. Please review your sort field and order.");
                }

                var results = await query.Select(b => new BookSearchResultsData
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Genre = b.Genre,
                    Description = b.Description,
                    PublishedDate = b.PublishedDate,
                    Pages = b.Pages
                }).ToListAsync();

                _logger.LogInformation("Search completed. Found {Count} books.", results.Count);

                return new BookSearchResponse
                {
                    Results = results
                };
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (InvalidOperationException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error occurred during book search.");
                throw new ApplicationException("An unexpected error occurred while processing your search. Please try again later.");
            }
        }
    }
}
