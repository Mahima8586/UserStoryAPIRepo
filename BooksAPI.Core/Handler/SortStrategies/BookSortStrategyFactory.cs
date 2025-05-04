// File: BookSortStrategyFactory.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public class BookSortStrategyFactory : IBookSortStrategyFactory
    {
        private readonly IBookSortStrategy _genericStrategy;

        public BookSortStrategyFactory(IBookSortStrategy genericStrategy)
        {
            _genericStrategy = genericStrategy;
        }

        public IBookSortStrategy GetStrategy() => _genericStrategy;
    }


}
