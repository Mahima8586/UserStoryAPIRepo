using System;
using System.Linq;
using System.Linq.Expressions;
using BooksAPI.Infrastructure.BooksDB.Entities;

namespace BooksAPI.Core.RequestHandler.SortStrategies
{
    public class GenericSortStrategy : IBookSortStrategy
    {
        public IQueryable<Books> ApplySort(IQueryable<Books> query, string sortField, string sortOrder, string keyword = null)
        {
            if (string.IsNullOrWhiteSpace(sortField))
                return query.OrderBy(b => b.Title); 

            var param = Expression.Parameter(typeof(Books), "b");
            var property = typeof(Books).GetProperty(sortField, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"'{sortField}' is not a valid sort field.");

            var propertyAccess = Expression.MakeMemberAccess(param, property);
            var orderByExpression = Expression.Lambda(propertyAccess, param);

            string methodName = sortOrder?.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { typeof(Books), property.PropertyType },
                query.Expression, Expression.Quote(orderByExpression));

            return query.Provider.CreateQuery<Books>(resultExpression);
        }
    }
}
