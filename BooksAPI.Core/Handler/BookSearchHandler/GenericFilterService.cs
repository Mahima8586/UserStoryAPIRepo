using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BooksAPI.Infrastructure.BooksDB.Entities;
using Microsoft.Extensions.Logging;

namespace BooksAPI.Core.Handler.BookSearchHandler
{
    public class GenericFilterService<T> where T : class
    {
        private readonly ILogger<GenericFilterService<T>> _logger;

        public GenericFilterService(ILogger<GenericFilterService<T>> logger)
        {
            _logger = logger;
        }

        public IQueryable<T> ApplyFilters(IQueryable<T> query, Dictionary<string, string> filters)
        {
            if (query == null)
            {
                _logger.LogWarning("ApplyFilters was called with a null query.");
                throw new ArgumentNullException(nameof(query), "Filter source cannot be null.");
            }

            if (filters == null || !filters.Any())
            {
                _logger.LogInformation("No filters provided — returning unfiltered query.");
                return query;
            }

            foreach (var filter in filters)
            {
                try
                {
                    var propertyInfo = typeof(T).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (propertyInfo == null)
                    {
                        _logger.LogWarning("Property '{Property}' not found on type '{Type}'. Skipping filter.", filter.Key, typeof(T).Name);
                        continue;
                    }

                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, propertyInfo);

                    object value;

                    try
                    {
                        value = Convert.ChangeType(filter.Value, propertyInfo.PropertyType);
                    }
                    catch (Exception convertEx)
                    {
                        _logger.LogWarning(convertEx, "Failed to convert filter value '{Value}' to type '{Type}' for property '{Property}'. Skipping.", filter.Value, propertyInfo.PropertyType.Name, filter.Key);
                        continue;
                    }

                    var constant = Expression.Constant(value);
                    var equals = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

                    query = query.Where(lambda);

                    _logger.LogInformation("Applied filter: {Property} = {Value}", filter.Key, filter.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error applying filter: {Key} = {Value}", filter.Key, filter.Value);
                    throw new InvalidOperationException($"A filter could not be applied for '{filter.Key}'. Please verify the field and value format.");
                }
            }

            return query;
        }
    }
}
