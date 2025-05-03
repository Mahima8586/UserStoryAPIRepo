using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public static class BookSortStrategyFactory
    {
        public static IBookSortStrategy GetStrategy(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "title_asc" => new TitleAscSort(),
                
                "pages_asc" => new PagesAscSort(),
                "relevance" => new RelevanceSort(),
                _ => new TitleAscSort()
            };
        }
    }

}
