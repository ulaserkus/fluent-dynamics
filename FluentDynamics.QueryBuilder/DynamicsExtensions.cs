using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder
{
    public static class DynamicsExtensions
    {
        public static QueryExpressionBuilder Clone(this QueryExpressionBuilder builder)
        {
            return new QueryExpressionBuilder(builder._query.EntityName)
            {
                _query = (QueryExpression)builder._query.Clone()
            };
        }
        public static QueryExpression Clone(this QueryExpression queryExpression)
        {
            return (QueryExpression)queryExpression.Clone();
        }

        public static List<Entity> ToList(this EntityCollection entities)
        {
            return entities?.Entities?.ToList() ?? new List<Entity>();
        }

        public static Task<List<Entity>> ToListAsync(this EntityCollection entities)
        {
            return Task.Run(() => ToList(entities));
        }

        public static Entity[] ToArray(this EntityCollection entities)
        {
            return entities?.Entities?.ToArray() ?? Array.Empty<Entity>();
        }

        public static Task<Entity[]> ToArrayAsync(this EntityCollection entities)
        {
            return Task.Run(() => ToArray(entities));
        }

        public static Entity FirstOrDefault(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.FirstOrDefault(predicate);
        }

        public static Task<Entity> FirstOrDefaultAsync(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return Task.Run(() => FirstOrDefault(entities, predicate));
        }

        public static Entity SingleOrDefault(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.SingleOrDefault(predicate);
        }

        public static Task<Entity> SingleOrDefaultAsync(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return Task.Run(() => SingleOrDefault(entities, predicate));
        }

        public static IEnumerable<Entity> Where(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return entities?.Entities?.Where(predicate) ?? Enumerable.Empty<Entity>();
        }

        public static Task<IEnumerable<Entity>> WhereAsync(this EntityCollection entities, Func<Entity, bool> predicate)
        {
            return Task.Run(() => Where(entities, predicate));
        }

        public static IEnumerable<TResult> Select<TResult>(this EntityCollection entities, Func<Entity, TResult> selector)
        {
            return entities?.Entities?.Select(selector) ?? Enumerable.Empty<TResult>();
        }

        public static Task<IEnumerable<TResult>> SelectAsync<TResult>(this EntityCollection entities, Func<Entity, TResult> selector)
        {
            return Task.Run(() => Select(entities, selector));
        }

        public static List<T> ToTypedList<T>(this EntityCollection entities) where T : Entity
        {
            return entities?.Entities?.OfType<T>().ToList() ?? new List<T>();
        }

        public static Task<List<T>> ToTypedListAsync<T>(this EntityCollection entities) where T : Entity
        {
            return Task.Run(() => ToTypedList<T>(entities));
        }

        public static T TryGet<T>(this Entity entity, string attributeName, T defaultValue = default)
        {
            if (entity != null && entity.Contains(attributeName) && entity[attributeName] is T value)
                return value;
            return defaultValue;
        }

        public static Task<T> TryGetAsync<T>(this Entity entity, string attributeName, T defaultValue = default)
        {
            return Task.Run(() => TryGet<T>(entity, attributeName, defaultValue));
        }
    }
}