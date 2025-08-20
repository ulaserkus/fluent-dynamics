using FluentDynamics.QueryBuilder.Builders;
using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for Dynamics 365/Dataverse entities and query results.
    /// Enhances the fluent query API with LINQ-like operations and convenience methods.
    /// </summary>
    public static class DynamicsExtensions
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
        /// Converts an EntityCollection to a List of Entity objects
        /// </summary>
        /// <param name="entities">The entity collection to convert</param>
        /// <returns>A List containing the entities, or an empty list if the collection is null</returns>
        public static List<Entity> ToList(this EntityCollection entities)
        {
            return entities?.Entities?.ToList() ?? new List<Entity>();
        }

        /// <summary>
        /// Asynchronously converts an EntityCollection to a List of Entity objects
        /// </summary>
        /// <param name="entities">The entity collection to convert</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning a List containing the entities</returns>
        public static Task<List<Entity>> ToListAsync(this EntityCollection entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.ToList(), cancellationToken);
        }

        /// <summary>
        /// Converts an EntityCollection to an array of Entity objects
        /// </summary>
        /// <param name="entities">The entity collection to convert</param>
        /// <returns>An array containing the entities, or an empty array if the collection is null</returns>
        public static Entity[] ToArray(this EntityCollection entities)
        {
            return entities?.Entities?.ToArray() ?? Array.Empty<Entity>();
        }

        /// <summary>
        /// Asynchronously converts an EntityCollection to an array of Entity objects
        /// </summary>
        /// <param name="entities">The entity collection to convert</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning an array containing the entities</returns>
        public static Task<Entity[]> ToArrayAsync(this EntityCollection entities, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.ToArray(), cancellationToken);
        }

        /// <summary>
        /// Returns the first entity that satisfies a condition or null if no such entity is found
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <returns>The first entity that satisfies the condition, or null</returns>
        public static Entity FirstOrDefault(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Asynchronously returns the first entity that satisfies a condition or null if no such entity is found
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning the first entity that satisfies the condition, or null</returns>
        public static Task<Entity> FirstOrDefaultAsync(this EntityCollection entities, Func<Entity, bool> predicate, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.FirstOrDefault(predicate));
        }

        /// <summary>
        /// Returns the only entity that satisfies a condition or null if no such entity is found
        /// Throws an exception if more than one entity satisfies the condition
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <returns>The only entity that satisfies the condition, or null</returns>
        public static Entity SingleOrDefault(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Asynchronously returns the only entity that satisfies a condition or null if no such entity is found
        /// Throws an exception if more than one entity satisfies the condition
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning the only entity that satisfies the condition, or null</returns>
        public static Task<Entity> SingleOrDefaultAsync(this EntityCollection entities, Func<Entity, bool> predicate, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.SingleOrDefault(predicate));
        }

        /// <summary>
        /// Filters entities based on a predicate
        /// </summary>
        /// <param name="entities">The entity collection to filter</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <returns>An enumerable containing entities that satisfy the condition</returns>
        public static IEnumerable<Entity> Where(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.Where(predicate) ?? Enumerable.Empty<Entity>();
        }

        /// <summary>
        /// Asynchronously filters entities based on a predicate
        /// </summary>
        /// <param name="entities">The entity collection to filter</param>
        /// <param name="predicate">A function to test each entity for a condition</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning an enumerable containing entities that satisfy the condition</returns>
        public static Task<IEnumerable<Entity>> WhereAsync(this EntityCollection entities, Func<Entity, bool> predicate, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.Where(predicate));
        }

        /// <summary>
        /// Projects each entity in a collection into a new form
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the selector</typeparam>
        /// <param name="entities">The entity collection to project</param>
        /// <param name="selector">A transform function to apply to each entity</param>
        /// <returns>An enumerable containing the projected results</returns>
        public static IEnumerable<TResult> Select<TResult>(this EntityCollection entities, Func<Entity, TResult> selector)
        {
            return entities?.Entities?.Select(selector) ?? Enumerable.Empty<TResult>();
        }

        /// <summary>
        /// Asynchronously projects each entity in a collection into a new form
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the selector</typeparam>
        /// <param name="entities">The entity collection to project</param>
        /// <param name="selector">A transform function to apply to each entity</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning an enumerable containing the projected results</returns>
        public static Task<IEnumerable<TResult>> SelectAsync<TResult>(this EntityCollection entities, Func<Entity, TResult> selector, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entities.Select(selector), cancellationToken);
        }

        /// <summary>
        /// Converts an EntityCollection to a List of typed entity objects
        /// </summary>
        /// <typeparam name="T">The specific entity type to convert to</typeparam>
        /// <param name="entities">The entity collection to convert</param>
        /// <returns>A List containing the typed entities, or an empty list if the collection is null</returns>
        public static List<T> ToTypedList<T>(this EntityCollection entities) where T : Entity
        {
            return entities?.Entities?.OfType<T>().ToList() ?? new List<T>();
        }

        /// <summary>
        /// Asynchronously converts an EntityCollection to a List of typed entity objects
        /// </summary>
        /// <typeparam name="T">The specific entity type to convert to</typeparam>
        /// <param name="entities">The entity collection to convert</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning a List containing the typed entities</returns>
        public static Task<List<T>> ToTypedListAsync<T>(this EntityCollection entities, CancellationToken cancellationToken = default) where T : Entity
        {
            return Task.Run(() => entities.ToTypedList<T>(), cancellationToken);
        }

        /// <summary>
        /// Safely retrieves a typed attribute value from an entity
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute value</typeparam>
        /// <param name="entity">The entity containing the attribute</param>
        /// <param name="attributeName">The logical name of the attribute to retrieve</param>
        /// <param name="defaultValue">The default value to return if the attribute doesn't exist or is of wrong type</param>
        /// <returns>The attribute value as the specified type, or the default value</returns>
        public static T TryGet<T>(this Entity entity, string attributeName, T defaultValue = default)
        {
            if (entity != null && entity.Contains(attributeName) && entity[attributeName] is T value)
                return value;
            return defaultValue;
        }

        /// <summary>
        /// Asynchronously retrieves a typed attribute value from an entity
        /// </summary>
        /// <typeparam name="T">The expected type of the attribute value</typeparam>
        /// <param name="entity">The entity containing the attribute</param>
        /// <param name="attributeName">The logical name of the attribute to retrieve</param>
        /// <param name="defaultValue">The default value to return if the attribute doesn't exist or is of wrong type</param>
        /// <param name="cancellationToken">A token to cancel the operation</param>
        /// <returns>A task returning the attribute value as the specified type, or the default value</returns>
        public static Task<T> TryGetAsync<T>(this Entity entity, string attributeName, T defaultValue = default, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => entity.TryGet(attributeName, defaultValue), cancellationToken);
        }

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