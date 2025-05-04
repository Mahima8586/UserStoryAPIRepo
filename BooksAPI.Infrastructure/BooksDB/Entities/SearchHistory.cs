using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Infrastructure.BooksDB.Entities
{
    public class SearchHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SearchTerm { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public DateTime? PublishedDate { get; set; }
        public int? Pages { get; set; }
        public int? MinPages { get; set; }
        public int? MaxPages { get; set; }
        public DateTime? PublishedFrom { get; set; }
        public DateTime? PublishedTo { get; set; }
        public string SortBy { get; set; }
        public DateTime SearchDate { get; set; } 
    }

}
