// File: IBookSortStrategy.cs
using System.Linq;
using BooksAPI.Infrastructure.BooksDB.Entities;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public interface IBookSortStrategy
    {
        IQueryable<Books> ApplySort(IQueryable<Books> query, string sortField, string sortOrder, string keyword = null);
    }

}
