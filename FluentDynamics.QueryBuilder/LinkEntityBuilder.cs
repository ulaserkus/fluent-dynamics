using Microsoft.Xrm.Sdk.Query;
using System;

namespace FluentDynamics.QueryBuilder
{
    /// <summary>
    /// Builder for configuring link-entity (join) operations in a Dynamics 365/Dataverse query
    /// </summary>
    public class LinkEntityBuilder
    {
        internal LinkEntity _linkEntity;

        /// <summary>
        /// Initializes a new instance of the LinkEntityBuilder
        /// </summary>
        /// <param name="linkEntity">The link entity to configure</param>
        internal LinkEntityBuilder(LinkEntity linkEntity)
        {
            _linkEntity = linkEntity;
        }

        /// <summary>
        /// Specifies which columns to include from the linked entity
        /// </summary>
        /// <param name="attributes">Names of attributes to include</param>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder Select(params string[] attributes)
        {
            _linkEntity.Columns.AddColumns(attributes);
            return this;
        }

        /// <summary>
        /// Includes all columns from the linked entity in the query results
        /// </summary>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder SelectAll()
        {
            _linkEntity.Columns = new ColumnSet(true);
            return this;
        }

        /// <summary>
        /// Sets an alias for the linked entity to prefix column names in the result set
        /// </summary>
        /// <param name="alias">The alias to use for the linked entity</param>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder As(string alias)
        {
            _linkEntity.EntityAlias = alias;
            return this;
        }

        /// <summary>
        /// Forces the query optimizer to use a specific index seek for the linked entity
        /// </summary>
        /// <param name="forceSeek">The index name to use for forced seek operation</param>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder ForceSeek(string forceSeek)
        {
            _linkEntity.ForceSeek = forceSeek;
            return this;
        }

        /// <summary>
        /// Adds a sort order to the linked entity results
        /// </summary>
        /// <param name="attribute">The attribute to sort on</param>
        /// <param name="orderType">The sort direction (Ascending or Descending)</param>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder OrderBy(string attribute, OrderType orderType = OrderType.Ascending)
        {
            _linkEntity.Orders.Add(new OrderExpression(attribute, orderType));
            return this;
        }

        /// <summary>
        /// Adds a filter group to the query using a fluent <see cref="FilterBuilder"/> configuration.
        /// Use this method to define complex filter logic (AND/OR/conditions) for the main query criteria.
        /// </summary>
        /// <param name="filterConfig">An action to configure the filter group via <see cref="FilterBuilder"/>.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public LinkEntityBuilder Where(Action<FilterBuilder> filterConfig)
        {
            var builder = new FilterBuilder(LogicalOperator.And);
            filterConfig(builder);
            _linkEntity.LinkCriteria = builder.ToExpression();
            return this;
        }

        /// <summary>
        /// Adds a nested link-entity (join) to this linked entity
        /// </summary>
        /// <param name="toEntity">The target entity to join</param>
        /// <param name="fromAttribute">The attribute from this linked entity</param>
        /// <param name="toAttribute">The attribute from the target entity</param>
        /// <param name="joinType">The type of join (Inner, Outer, etc.)</param>
        /// <param name="linkBuilder">Action to configure the nested link entity</param>
        /// <returns>The builder instance for method chaining</returns>
        public LinkEntityBuilder Link(
            string toEntity,
            string fromAttribute,
            string toAttribute,
            JoinOperator joinType,
            Action<LinkEntityBuilder> linkBuilder)
        {
            var link = new LinkEntity(_linkEntity.LinkToEntityName, toEntity, fromAttribute, toAttribute, joinType);
            var builder = new LinkEntityBuilder(link);
            linkBuilder(builder);
            _linkEntity.LinkEntities.Add(link);
            return this;
        }
    }
}