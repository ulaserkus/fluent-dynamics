using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Provides extension methods for EntityCollection class.
    /// </summary>
    public static class EntityCollectionExtensions
    {
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
        /// Converts an EntityCollection to an array of Entity objects
        /// </summary>
        /// <param name="entities">The entity collection to convert</param>
        /// <returns>An array containing the entities, or an empty array if the collection is null</returns>
        public static Entity[] ToArray(this EntityCollection entities)
        {
            return entities?.Entities?.ToArray() ?? Array.Empty<Entity>();
        }

        /// <summary>
        /// Returns the first entity that satisfies a condition or null if no such entity is found.
        /// If predicate is null, returns the first entity in the collection.
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition, or null to return the first entity</param>
        /// <returns>The first entity that satisfies the condition, or null</returns>
        public static Entity? FirstOrDefault(this EntityCollection entities, Func<Entity, bool>? predicate = null)
        {
            if (entities?.Entities == null)
                return null;

            return predicate == null
                ? entities.Entities.FirstOrDefault()
                : entities.Entities.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Returns the only entity that satisfies a condition or null if no such entity is found.
        /// Throws an exception if more than one entity satisfies the condition.
        /// If predicate is null, returns the only entity in the collection or null if empty or multiple entities exist.
        /// </summary>
        /// <param name="entities">The entity collection to search</param>
        /// <param name="predicate">A function to test each entity for a condition, or null to check if collection has exactly one entity</param>
        /// <returns>The only entity that satisfies the condition, or null</returns>
        public static Entity? SingleOrDefault(this EntityCollection entities, Func<Entity, bool>? predicate = null)
        {
            if (entities?.Entities == null)
                return null;

            return predicate == null
                ? entities.Entities.SingleOrDefault()
                : entities.Entities.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Filters entities based on a predicate.
        /// If predicate is null, returns all entities.
        /// </summary>
        /// <param name="entities">The entity collection to filter</param>
        /// <param name="predicate">A function to test each entity for a condition, or null to return all entities</param>
        /// <returns>An enumerable containing entities that satisfy the condition</returns>
        public static IEnumerable<Entity> Where(this EntityCollection entities, Func<Entity, bool>? predicate = null)
        {
            if (entities?.Entities == null)
                return Enumerable.Empty<Entity>();

            return predicate == null
                ? entities.Entities
                : entities.Entities.Where(predicate);
        }

        /// <summary>
        /// Projects each entity in a collection into a new form.
        /// If selector is null, throws ArgumentNullException.
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the selector</typeparam>
        /// <param name="entities">The entity collection to project</param>
        /// <param name="selector">A transform function to apply to each entity</param>
        /// <returns>An enumerable containing the projected results</returns>
        public static IEnumerable<TResult> Select<TResult>(this EntityCollection entities, Func<Entity, TResult> selector)
        {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector), "Transform function cannot be null");

            return entities?.Entities?.Select(selector) ?? Enumerable.Empty<TResult>();
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
    }
}