using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksAPI.Infrastructure.BooksDB.Entities;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public class TitleAscSort : IBookSortStrategy
    {
        public IQueryable<Books> ApplySort(IQueryable<Books> query, string keyword = null)
            => query.OrderBy(b => b.Title);
    }

}
