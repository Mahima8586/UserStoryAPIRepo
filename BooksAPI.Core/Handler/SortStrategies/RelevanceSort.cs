using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksAPI.Infrastructure.BooksDB.Entities;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public class RelevanceSort : IBookSortStrategy
    {
        public IQueryable<Books> ApplySort(IQueryable<Books> query, string keyword = null)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return query;

            return query.OrderByDescending(b =>
                (b.Title.Contains(keyword) ? 3 : 0) +
                (b.Author.Contains(keyword) ? 2 : 0) +
                (b.Description.Contains(keyword) ? 1 : 0));
        }
    }

}
