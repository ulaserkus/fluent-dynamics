using FluentDynamics.QueryBuilder.Builders;

namespace FluentDynamics.QueryBuilder
{
    /// <summary>
    /// Entry point for the Dynamics 365/Dataverse fluent query API.
    /// Provides factory methods to create query builders for various entity types.
    /// </summary>
    public static class Query
    {
        /// <summary>
        /// Creates a new query builder for the specified entity type.
        /// </summary>
        /// <param name="entityName">The logical name of the entity to query.</param>
        /// <returns>A new QueryExpressionBuilder instance configured for the specified entity.</returns>
        public static QueryExpressionBuilder For(string entityName)
        {
            return new QueryExpressionBuilder(entityName);
        }
    }
}