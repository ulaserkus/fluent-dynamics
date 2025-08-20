using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for QueryExpression class.
    /// </summary>
    public static class QueryExpressionExtensions
    {
        /// <summary>
        /// Creates a shallow clone of a QueryExpression
        /// </summary>
        /// <param name="queryExpression">The query expression to clone</param>
        /// <returns>A new instance with the same configuration</returns>
        public static QueryExpression ShallowClone(this QueryExpression queryExpression)
        {
            var query = new QueryExpression
            {
                ColumnSet = queryExpression.ColumnSet,
                Criteria = queryExpression.Criteria,
                EntityName = queryExpression.EntityName,
                NoLock = queryExpression.NoLock,
                Distinct = queryExpression.Distinct,
                DataSource = queryExpression.DataSource,
                PageInfo = queryExpression.PageInfo,
                TopCount = queryExpression.TopCount,
                ExtensionData = queryExpression.ExtensionData,
                ForceSeek = queryExpression.ForceSeek,
                QueryHints = queryExpression.QueryHints,
                SubQueryExpression = queryExpression.SubQueryExpression,
            };

            query.LinkEntities.AddRange(queryExpression.LinkEntities);
            query.Orders.AddRange(queryExpression.Orders);
            return query;
        }

        /// <summary>
        /// Creates a deep clone of a QueryExpression, including all nested objects and collections.
        /// Unlike the shallow Clone method, this creates completely independent copies 
        /// of all reference-type properties (ColumnSet, Criteria, etc.) and collections (LinkEntities, Orders).
        /// Modifications to the cloned query will not affect the original query.
        /// </summary>
        /// <param name="queryExpression">The query expression to deep clone</param>
        /// <returns>A new instance with completely independent copies of all nested objects</returns>
        public static QueryExpression DeepClone(this QueryExpression queryExpression)
        {
            if (queryExpression == null)
                return null;

            var query = new QueryExpression
            {
                EntityName = queryExpression.EntityName,
                NoLock = queryExpression.NoLock,
                Distinct = queryExpression.Distinct,
                DataSource = queryExpression.DataSource,
                TopCount = queryExpression.TopCount,
                ExtensionData = queryExpression.ExtensionData,
                ForceSeek = queryExpression.ForceSeek,
                QueryHints = queryExpression.QueryHints,
            };

            // Deep clone ColumnSet
            if (queryExpression.ColumnSet != null)
            {
                query.ColumnSet = new ColumnSet();
                if (queryExpression.ColumnSet.AllColumns)
                {
                    query.ColumnSet.AllColumns = true;
                }
                else
                {
                    query.ColumnSet.AddColumns(queryExpression.ColumnSet.Columns.ToArray());
                }
            }

            // Deep clone Criteria
            if (queryExpression.Criteria != null)
            {
                query.Criteria = DeepCloneFilterExpression(queryExpression.Criteria);
            }

            // Deep clone PageInfo
            if (queryExpression.PageInfo != null)
            {
                query.PageInfo = new PagingInfo
                {
                    Count = queryExpression.PageInfo.Count,
                    PageNumber = queryExpression.PageInfo.PageNumber,
                    PagingCookie = queryExpression.PageInfo.PagingCookie,
                    ReturnTotalRecordCount = queryExpression.PageInfo.ReturnTotalRecordCount
                };
            }

            // Deep clone SubQueryExpression
            if (queryExpression.SubQueryExpression != null)
            {
                query.SubQueryExpression = queryExpression.SubQueryExpression.DeepClone();
            }

            // Deep clone LinkEntities
            foreach (var linkEntity in queryExpression.LinkEntities)
            {
                query.LinkEntities.Add(DeepCloneLinkEntity(linkEntity));
            }

            // Deep clone Orders
            foreach (var order in queryExpression.Orders)
            {
                query.Orders.Add(new OrderExpression(order.AttributeName, order.OrderType));
            }

            return query;
        }

        /// <summary>
        /// Creates a lightweight clone of a QueryExpression optimized for pagination.
        /// Only the pagination-related properties are modified, keeping the rest shared.
        /// </summary>
        /// <param name="queryExpression">The query expression to clone for pagination</param>
        /// <param name="pageNumber">The page number to set</param>
        /// <param name="pageSize">The page size to set</param>
        /// <param name="pagingCookie">The paging cookie (optional)</param>
        /// <returns>A new instance with updated pagination properties</returns>
        public static QueryExpression CloneForPagination(
            this QueryExpression queryExpression,
            int pageNumber,
            int pageSize,
            string pagingCookie = null)
        {
            if (queryExpression == null)
                return null;

            // Create a new instance with same core settings
            var query = queryExpression.ShallowClone();
            query.PageInfo = new PagingInfo
            {
                PageNumber = pageNumber,
                Count = pageSize,
                PagingCookie = pagingCookie
            };

            return query;
        }

        /// <summary>
        /// Deep clones a FilterExpression, including all nested conditions and filters
        /// </summary>
        private static FilterExpression DeepCloneFilterExpression(FilterExpression filterExpression)
        {
            if (filterExpression == null)
                return null;

            var clonedFilter = new FilterExpression
            {
                FilterOperator = filterExpression.FilterOperator,
                IsQuickFindFilter = filterExpression.IsQuickFindFilter
            };

            // Deep clone conditions
            foreach (var condition in filterExpression.Conditions)
            {
                var clonedCondition = new ConditionExpression
                {
                    AttributeName = condition.AttributeName,
                    Operator = condition.Operator,
                    EntityName = condition.EntityName
                };

                // Deep clone values
                if (condition.Values != null && condition.Values.Count > 0)
                {
                    clonedCondition.Values.AddRange(condition.Values);
                }

                clonedFilter.Conditions.Add(clonedCondition);
            }

            // Deep clone nested filters
            foreach (var nestedFilter in filterExpression.Filters)
            {
                clonedFilter.Filters.Add(DeepCloneFilterExpression(nestedFilter));
            }

            return clonedFilter;
        }

        /// <summary>
        /// Deep clones a LinkEntity, including all nested properties and child LinkEntities
        /// </summary>
        private static LinkEntity DeepCloneLinkEntity(LinkEntity linkEntity)
        {
            if (linkEntity == null)
                return null;

            var clonedLink = new LinkEntity
            {
                LinkFromEntityName = linkEntity.LinkFromEntityName,
                LinkToEntityName = linkEntity.LinkToEntityName,
                LinkFromAttributeName = linkEntity.LinkFromAttributeName,
                LinkToAttributeName = linkEntity.LinkToAttributeName,
                JoinOperator = linkEntity.JoinOperator,
                EntityAlias = linkEntity.EntityAlias,
                ForceSeek = linkEntity.ForceSeek
            };

            // Deep clone ColumnSet
            if (linkEntity.Columns != null)
            {
                clonedLink.Columns = new ColumnSet();
                if (linkEntity.Columns.AllColumns)
                {
                    clonedLink.Columns.AllColumns = true;
                }
                else
                {
                    clonedLink.Columns.AddColumns(linkEntity.Columns.Columns.ToArray());
                }
            }

            // Deep clone LinkCriteria
            if (linkEntity.LinkCriteria != null)
            {
                clonedLink.LinkCriteria = DeepCloneFilterExpression(linkEntity.LinkCriteria);
            }

            // Deep clone Orders
            foreach (var order in linkEntity.Orders)
            {
                clonedLink.Orders.Add(new OrderExpression(order.AttributeName, order.OrderType));
            }

            // Deep clone nested LinkEntities
            foreach (var nestedLink in linkEntity.LinkEntities)
            {
                clonedLink.LinkEntities.Add(DeepCloneLinkEntity(nestedLink));
            }

            return clonedLink;
        }
    }
}