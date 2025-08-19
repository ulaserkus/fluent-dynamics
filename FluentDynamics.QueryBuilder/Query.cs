namespace FluentDynamics.QueryBuilder
{
    /// <summary>
    /// Entry point for the Dynamics 365/Dataverse fluent query API.
    /// Provides factory methods to create query builders for various entity types.
    /// </summary>
    /// <example>
    /// <code>
    /// // Example usage:
    /// var query = Query.For("account")
    ///     .Select("name", "accountnumber")
    ///     .Where("statecode", ConditionOperator.Equal, 0)
    ///     .OrderBy("name");
    /// 
    /// var results = query.RetrieveMultiple(organizationService);
    /// </code>
    /// </example>
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