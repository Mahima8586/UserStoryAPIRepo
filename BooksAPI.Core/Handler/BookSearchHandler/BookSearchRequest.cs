using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler
{
    public class BookSearchRequest
    {
        public int? Id { get; set; } = null;

        public string? Query { get; set; } = null;
        public string? Title { get; set; } = null;
        public string? Author { get; set; } = null;
        public string? Genre { get; set; } = null;
        public string? Description { get; set; } = null;

        public DateTime? PublishedDate { get; set; } = null;

        public int? Pages { get; set; } = null;
        public int? MinPages { get; set; } = null;
        public int? MaxPages { get; set; } = null;

        public DateTime? PublishedFrom { get; set; } = null;
        public DateTime? PublishedTo { get; set; } = null;

        public string SortBy { get; set; } = "relevance";
    }

}
