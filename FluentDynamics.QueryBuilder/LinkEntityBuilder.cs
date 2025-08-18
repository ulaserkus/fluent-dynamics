using Microsoft.Xrm.Sdk.Query;
using System;

namespace FluentDynamics.QueryBuilder
{
    public class LinkEntityBuilder
    {
        private readonly LinkEntity _linkEntity;

        public LinkEntityBuilder(LinkEntity linkEntity)
        {
            _linkEntity = linkEntity;
        }

        public LinkEntityBuilder Select(params string[] attributes)
        {
            _linkEntity.Columns.AddColumns(attributes);
            return this;
        }

        public LinkEntityBuilder SelectAll()
        {
            _linkEntity.Columns = new ColumnSet(true);
            return this;
        }

        public LinkEntityBuilder Where(string attribute, ConditionOperator op, object value)
        {
            _linkEntity.LinkCriteria.AddCondition(attribute, op, value);
            return this;
        }

        // Nested Link Desteği
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

        // AND/OR desteği
        public LinkEntityBuilder And(Action<LinkEntityBuilder> nested)
        {
            var filter = new FilterExpression(LogicalOperator.And);
            var nestedBuilder = new LinkEntityBuilder(_linkEntity);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._linkEntity.LinkCriteria.Conditions)
                filter.Conditions.Add(cond);
            _linkEntity.LinkCriteria.AddFilter(filter);
            return this;
        }

        public LinkEntityBuilder Or(Action<LinkEntityBuilder> nested)
        {
            var filter = new FilterExpression(LogicalOperator.Or);
            var nestedBuilder = new LinkEntityBuilder(_linkEntity);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._linkEntity.LinkCriteria.Conditions)
                filter.Conditions.Add(cond);
            _linkEntity.LinkCriteria.AddFilter(filter);
            return this;
        }
    }
}