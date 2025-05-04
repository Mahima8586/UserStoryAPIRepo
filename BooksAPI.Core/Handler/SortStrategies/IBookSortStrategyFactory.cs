// File: IBookSortStrategyFactory.cs
using BooksAPI.Core.RequestHandler.SortStrategies;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public interface IBookSortStrategyFactory
    {
        IBookSortStrategy GetStrategy();
    }

}
