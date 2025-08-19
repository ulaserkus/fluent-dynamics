using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder
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
        public static QueryExpressionBuilder Clone(this QueryExpressionBuilder builder)
        {
            return new QueryExpressionBuilder(builder._query.EntityName)
            {
                _query = (QueryExpression)builder._query.Clone()
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
            return Task.Run(() => ToList(entities), cancellationToken);
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
            return Task.Run(() => ToArray(entities), cancellationToken);
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
            return Task.Run(() => FirstOrDefault(entities, predicate));
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
            return Task.Run(() => SingleOrDefault(entities, predicate));
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
            return Task.Run(() => Where(entities, predicate));
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
            return Task.Run(() => Select(entities, selector), cancellationToken);
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
            return Task.Run(() => ToTypedList<T>(entities), cancellationToken);
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
            return Task.Run(() => TryGet<T>(entity, attributeName, defaultValue), cancellationToken);
        }

        /// <summary>
        /// Creates a deep clone of a QueryExpression
        /// </summary>
        /// <param name="queryExpression">The query expression to clone</param>
        /// <returns>A new instance with the same configuration</returns>
        internal static QueryExpression Clone(this QueryExpression queryExpression)
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
    }
}