using Microsoft.Xrm.Sdk.Query;
using System;

namespace FluentDynamics.QueryBuilder.Builders
{
    /// <summary>
    /// Fluent API builder for constructing nested filter groups in Dynamics 365/Dataverse queries.
    /// <para>
    /// Enables chaining of conditions and logical groups (AND/OR) for building complex filter expressions.
    /// </para>
    /// </summary>
    public class FilterBuilder
    {
        internal FilterExpression _filter;

        /// <summary>
        /// Initializes a new filter group with the specified logical operator (AND/OR).
        /// </summary>
        /// <param name="op">Logical operator for this filter group.</param>
        public FilterBuilder(LogicalOperator op)
        {
            _filter = new FilterExpression(op);
        }

        /// <summary>
        /// Adds a condition to the filter group.
        /// </summary>
        /// <param name="attribute">The attribute name to filter on.</param>
        /// <param name="op">The comparison operator.</param>
        /// <param name="value">The value to compare against (optional).</param>
        /// <returns>The builder instance for method chaining.</returns>
        public FilterBuilder Condition(string attribute, ConditionOperator op, object value = null)
        {
            if (value is null)
                _filter.AddCondition(attribute, op);
            else
                _filter.AddCondition(attribute, op, value);

            return this;
        }

        /// <summary>
        /// Adds a nested AND filter group to this filter group.
        /// </summary>
        /// <param name="nested">Action to configure the nested filter builder.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public FilterBuilder And(Action<FilterBuilder> nested)
        {
            var nestedBuilder = new FilterBuilder(LogicalOperator.And);
            nested(nestedBuilder);
            _filter.AddFilter(nestedBuilder._filter);
            return this;
        }

        /// <summary>
        /// Adds a nested OR filter group to this filter group.
        /// </summary>
        /// <param name="nested">Action to configure the nested filter builder.</param>
        /// <returns>The builder instance for method chaining.</returns>
        public FilterBuilder Or(Action<FilterBuilder> nested)
        {
            var nestedBuilder = new FilterBuilder(LogicalOperator.Or);
            nested(nestedBuilder);
            _filter.AddFilter(nestedBuilder._filter);
            return this;
        }

        /// <summary>
        /// Returns the built <see cref="FilterExpression"/> instance representing this filter group.
        /// </summary>
        /// <returns>The constructed <see cref="FilterExpression"/>.</returns>
        public FilterExpression ToExpression() => _filter;
    }
}