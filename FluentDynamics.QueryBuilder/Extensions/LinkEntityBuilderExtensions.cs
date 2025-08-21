using FluentDynamics.QueryBuilder.Builders;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Defines extension methods for the LinkEntityBuilder to simplify creating various types of joins.
    /// </summary>
    public static class LinkEntityBuilderExtensions
    {
        /// <summary>
        /// Creates a link entity with an Inner join and applies the provided configuration action.
        /// </summary>
        public static LinkEntityBuilder Inner(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Inner, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a LeftOuter join and applies the provided configuration action.
        /// </summary>
        public static LinkEntityBuilder LeftOuter(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.LeftOuter, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a Natural join and applies the provided configuration action.
        /// </summary>
        public static LinkEntityBuilder Natural(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Natural, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a MatchFirstRowUsingCrossApply join for better performance when only one matching row is needed.
        /// </summary>
        public static LinkEntityBuilder MatchFirstRowUsingCrossApply(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.MatchFirstRowUsingCrossApply, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an In join that can provide performance benefits using IN condition in WHERE clause.
        /// </summary>
        public static LinkEntityBuilder In(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.In, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an Exists join that can provide performance benefits using EXISTS condition in WHERE clause.
        /// </summary>
        public static LinkEntityBuilder Exists(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Exists, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with an Any join that restricts results to parent rows with any matching rows in the related table.
        /// </summary>
        public static LinkEntityBuilder Any(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.Any, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a NotAny join that restricts results to parent rows with no matching rows in the related table.
        /// </summary>
        public static LinkEntityBuilder NotAny(this LinkEntityBuilder builder, string toEntity,
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
        public static LinkEntityBuilder All(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.All, linkBuilder);
        }

        /// <summary>
        /// Creates a link entity with a NotAll join, which despite the name, is equivalent to Any join operation.
        /// </summary>
        public static LinkEntityBuilder NotAll(this LinkEntityBuilder builder, string toEntity,
            string fromAttribute,
            string toAttribute,
            Action<LinkEntityBuilder> linkBuilder)
        {
            return builder.Link(toEntity, fromAttribute, toAttribute, JoinOperator.NotAll, linkBuilder);
        }
    }
}