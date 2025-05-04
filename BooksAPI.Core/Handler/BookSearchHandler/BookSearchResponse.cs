using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler
{
    public class BookSearchResultsData
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }

        public string? Description { get; set; }

        public DateTime PublishedDate { get; set; }

        public int? Pages { get; set; }
    }
    public class BookSearchResponse
    {
        public string? Query { get; set; }

        public List<BookSearchResultsData> Results { get; set; }
    }
}
