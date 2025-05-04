using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Core.RequestHandler.BooksSearchHandler
{
    public class BookSearchRequest
    {
        public string Query { get; set; }
        public Dictionary<string, string> Filters { get; set; } = new();
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }


}
