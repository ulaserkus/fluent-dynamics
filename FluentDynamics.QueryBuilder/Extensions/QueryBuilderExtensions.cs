using FluentDynamics.QueryBuilder.Builders;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Linq;
using System.Text;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for QueryExpressionBuilder class.
    /// </summary>
    public static class QueryBuilderExtensions
    {
        /// <summary>
        /// Creates a link entity with an Inner join and applies the provided configuration action.
        /// </summary>
        public static QueryExpressionBuilder Inner(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Inner, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a LeftOuter join and applies the provided configuration action.
        /// </summary>
        public static QueryExpressionBuilder LeftOuter(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.LeftOuter, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a Natural join and applies the provided configuration action.
        /// </summary>
        public static QueryExpressionBuilder Natural(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Natural, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a MatchFirstRowUsingCrossApply join for better performance when only one matching row is needed.
        /// </summary>
        public static QueryExpressionBuilder MatchFirstRowUsingCrossApply(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.MatchFirstRowUsingCrossApply, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an In join that can provide performance benefits using IN condition in WHERE clause.
        /// </summary>
        public static QueryExpressionBuilder In(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.In, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an Exists join that can provide performance benefits using EXISTS condition in WHERE clause.
        /// </summary>
        public static QueryExpressionBuilder Exists(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Exists, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an Any join that restricts results to parent rows with any matching rows in the related table.
        /// </summary>
        public static QueryExpressionBuilder Any(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Any, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a NotAny join that restricts results to parent rows with no matching rows in the related table.
        /// </summary>
        public static QueryExpressionBuilder NotAny(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.NotAny, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an All join that filters records where rows with matching attribute values exist,
        /// but none of those matching rows satisfy the additional filters defined.
        /// </summary>
        public static QueryExpressionBuilder All(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.All, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a NotAll join, which despite the name, is equivalent to Any join operation.
        /// </summary>
        public static QueryExpressionBuilder NotAll(this QueryExpressionBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.NotAll, linkBuilder);
        }

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

        /// <summary>
        /// Returns a human-readable representation of the query for debugging
        /// </summary>
        /// <returns>A formatted string showing the query structure</returns>
        public static string DebugView(this QueryExpressionBuilder queryBuilder)
        {
            var sb = new StringBuilder();
            var query = queryBuilder._query;
            // Basic query info
            sb.AppendLine($"QueryExpression: {query.EntityName}");

            // Columns
            sb.AppendLine("Columns:");
            if (query.ColumnSet == null)
            {
                sb.AppendLine("  (none)");
            }
            else if (query.ColumnSet.AllColumns)
            {
                sb.AppendLine("  (all columns)");
            }
            else
            {
                foreach (var col in query.ColumnSet.Columns)
                {
                    sb.AppendLine($"  - {col}");
                }
            }

            // Criteria
            if (query.Criteria != null)
            {
                sb.AppendLine("Criteria:");
                AppendFilterDebugView(sb, query.Criteria, 1);
            }

            // Link entities
            if (query.LinkEntities.Count > 0)
            {
                sb.AppendLine("Link Entities:");
                foreach (var link in query.LinkEntities)
                {
                    AppendLinkEntityDebugView(sb, link, 1);
                }
            }

            // Orders
            if (query.Orders.Count > 0)
            {
                sb.AppendLine("Order By:");
                foreach (var order in query.Orders)
                {
                    sb.AppendLine($"  - {order.AttributeName} ({order.OrderType})");
                }
            }

            // Other properties
            if (query.TopCount > 0)
                sb.AppendLine($"Top Count: {query.TopCount}");
            if (query.PageInfo != null)
                sb.AppendLine($"Page: {query.PageInfo.PageNumber}, Size: {query.PageInfo.Count}");
            if (query.Distinct)
                sb.AppendLine("Distinct: Yes");
            if (query.NoLock)
                sb.AppendLine("NoLock: Yes");

            return sb.ToString();
        }

        /// <summary>
        /// Formats a filter expression for the debug view
        /// </summary>
        private static void AppendFilterDebugView(StringBuilder sb, FilterExpression filter, int indent)
        {
            string indentStr = new string(' ', indent * 2);

            // Show filter operator
            sb.AppendLine($"{indentStr}Operator: {filter.FilterOperator}");

            // Show conditions
            if (filter.Conditions.Count > 0)
            {
                sb.AppendLine($"{indentStr}Conditions:");
                foreach (var condition in filter.Conditions)
                {
                    string valueStr = "(none)";
                    if (condition.Values != null && condition.Values.Count > 0)
                    {
                        if (condition.Values.Count == 1)
                        {
                            if (condition.Values[0] is object[] valueArray)
                            {
                                valueStr = $"[{string.Join(", ", valueArray.Select(v => FormatValue(v)))}]";
                            }
                            else
                            {
                                valueStr = FormatValue(condition.Values[0]);
                            }
                        }
                        else
                        {
                            valueStr = $"[{string.Join(", ", condition.Values.Select(v => FormatValue(v)))}]";
                        }
                    }

                    sb.AppendLine($"{indentStr}  - {condition.AttributeName} {condition.Operator} {valueStr}");
                }
            }

            // Show nested filters
            if (filter.Filters.Count > 0)
            {
                sb.AppendLine($"{indentStr}Nested Filters:");
                int filterIndex = 1;
                foreach (var nestedFilter in filter.Filters)
                {
                    sb.AppendLine($"{indentStr}  Filter {filterIndex++}:");
                    AppendFilterDebugView(sb, nestedFilter, indent + 2);
                }
            }
        }

        /// <summary>
        /// Formats a link entity for the debug view
        /// </summary>
        private static void AppendLinkEntityDebugView(StringBuilder sb, LinkEntity link, int indent)
        {
            string indentStr = new string(' ', indent * 2);

            // Basic link info
            sb.AppendLine($"{indentStr}Link: {link.LinkFromEntityName}.{link.LinkFromAttributeName} -> " +
                          $"{link.LinkToEntityName}.{link.LinkToAttributeName} [{link.JoinOperator}]");

            // Show alias if present
            if (!string.IsNullOrEmpty(link.EntityAlias))
            {
                sb.AppendLine($"{indentStr}Alias: {link.EntityAlias}");
            }

            // Show columns
            if (link.Columns != null)
            {
                sb.AppendLine($"{indentStr}Columns:");
                if (link.Columns.AllColumns)
                {
                    sb.AppendLine($"{indentStr}  (all columns)");
                }
                else if (link.Columns.Columns.Count > 0)
                {
                    foreach (var column in link.Columns.Columns)
                    {
                        sb.AppendLine($"{indentStr}  - {column}");
                    }
                }
                else
                {
                    sb.AppendLine($"{indentStr}  (none)");
                }
            }

            // Show criteria
            if (link.LinkCriteria != null &&
                (link.LinkCriteria.Conditions.Count > 0 || link.LinkCriteria.Filters.Count > 0))
            {
                sb.AppendLine($"{indentStr}Criteria:");
                AppendFilterDebugView(sb, link.LinkCriteria, indent + 1);
            }

            // Show orders
            if (link.Orders.Count > 0)
            {
                sb.AppendLine($"{indentStr}Order By:");
                foreach (var order in link.Orders)
                {
                    sb.AppendLine($"{indentStr}  - {order.AttributeName} ({order.OrderType})");
                }
            }

            // Show nested link entities
            if (link.LinkEntities.Count > 0)
            {
                sb.AppendLine($"{indentStr}Nested Links:");
                foreach (var nestedLink in link.LinkEntities)
                {
                    AppendLinkEntityDebugView(sb, nestedLink, indent + 1);
                }
            }
        }

        /// <summary>
        /// Helper to format values for debug view
        /// </summary>
        private static string FormatValue(object value)
        {
            if (value == null)
                return "null";

            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd HH:mm:ss");

            if (value is Guid guid)
                return guid.ToString("B");

            if (value is EntityReference entityRef)
                return $"{entityRef.LogicalName}:{entityRef.Id}";

            if (value is Money money)
                return money.Value.ToString("C");

            if (value is OptionSetValue optionSet)
                return optionSet.Value.ToString();

            return value.ToString();
        }
    }
}