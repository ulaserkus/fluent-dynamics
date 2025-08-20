using FluentDynamics.QueryBuilder.Builders;
using Microsoft.Xrm.Sdk.Query;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for QueryExpressionBuilder class.
    /// </summary>
    public static class QueryBuilderExtensions
    {
        /// <summary>
        /// Creates a deep clone of a query builder instance
        /// </summary>
        /// <param name="builder">The query builder to clone</param>
        /// <returns>A new instance with the same query configuration</returns>
        public static QueryExpressionBuilder DeepClone(this QueryExpressionBuilder builder)
        {
            return new QueryExpressionBuilder(builder._query.EntityName)
            {
                _query = builder._query.DeepClone()
            };
        }

        /// <summary>
        /// Creates a shallow clone of a query builder instance
        /// </summary>
        /// <param name="builder">The query builder to clone</param>
        /// <returns>A new instance with the same query configuration</returns>
        public static QueryExpressionBuilder ShallowClone(this QueryExpressionBuilder builder)
        {
            return new QueryExpressionBuilder(builder._query.EntityName)
            {
                _query = builder._query.ShallowClone()
            };
        }

        /// <summary>
        /// Clones a QueryExpressionBuilder optimized for pagination
        /// </summary>
        /// <param name="builder">The builder to clone</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <param name="pagingCookie">Optional paging cookie from previous results</param>
        /// <returns>New builder instance configured for the specified page</returns>
        public static QueryExpressionBuilder CloneForPagination(
            this QueryExpressionBuilder builder,
            int pageNumber,
            int pageSize,
            string pagingCookie = null)
        {
            var clone = builder.ShallowClone();

            clone._query.PageInfo = new PagingInfo
            {
                PageNumber = pageNumber,
                Count = pageSize,
                PagingCookie = pagingCookie
            };

            return clone;
        }
    }
}