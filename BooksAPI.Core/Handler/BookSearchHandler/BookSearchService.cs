using System;
using System.Linq;
using BooksAPI.Infrastructure.BooksDB.Entities;
using Microsoft.Extensions.Logging;

public class BookSearchService
{
    private readonly ILogger<BookSearchService> _logger;

    public BookSearchService(ILogger<BookSearchService> logger)
    {
        _logger = logger;
    }

    public IQueryable<Books> ApplySearch(IQueryable<Books> query, string keyword)
    {
        if (query == null)
        {
            _logger.LogWarning("Search queryable source was null.");
            throw new ArgumentNullException(nameof(query), "Search source cannot be null.");
        }

        if (string.IsNullOrWhiteSpace(keyword))
        {
            _logger.LogInformation("No keyword provided — skipping keyword search.");
            return query;
        }

        try
        {
            if (!string.IsNullOrWhiteSpace(keyword) && keyword.All(char.IsLetterOrDigit) == false && !keyword.Any(char.IsLetter))
            {
                _logger.LogWarning("Invalid keyword format provided: {Keyword}", keyword);
                throw new InvalidOperationException("Please enter a valid text-based keyword for searching.");
            }

            keyword = keyword.ToLower();

            var filtered = query.Where(b =>
                b.Title.ToLower().Contains(keyword) ||
                b.Author.ToLower().Contains(keyword) ||
                b.Genre.ToLower().Contains(keyword) ||
                b.Description.ToLower().Contains(keyword));

            _logger.LogInformation("Keyword search applied for: '{Keyword}'", keyword);
            return filtered;
        }
        catch (InvalidOperationException invEx)
        {
            _logger.LogWarning(invEx, "User entered an invalid keyword: {Keyword}", keyword);
            throw; 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during keyword search for: '{Keyword}'", keyword);
            throw new ApplicationException("An error occurred while processing your search keyword. Please try again.");
        }
    }
}