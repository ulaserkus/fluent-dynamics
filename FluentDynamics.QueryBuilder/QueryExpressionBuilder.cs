using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder
{
    public class QueryExpressionBuilder
    {
        internal QueryExpression _query;

        public QueryExpressionBuilder(string entityName)
        {
            _query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet()
            };
        }

        public QueryExpressionBuilder Select(params string[] attributes)
        {
            _query.ColumnSet.AddColumns(attributes);
            return this;
        }

        public QueryExpressionBuilder SelectAll()
        {
            _query.ColumnSet = new ColumnSet(true);
            return this;
        }

        public QueryExpressionBuilder NoLock()
        {
            _query.NoLock = true;
            return this;
        }

        public QueryExpressionBuilder Where(string attribute, ConditionOperator op, object value)
        {
            _query.Criteria.AddCondition(attribute, op, value);
            return this;
        }

        public QueryExpressionBuilder And(Action<QueryExpressionBuilder> nested)
        {
            var filter = new FilterExpression(LogicalOperator.And);
            var nestedBuilder = new QueryExpressionBuilder(_query.EntityName);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._query.Criteria.Conditions)
                filter.Conditions.Add(cond);
            _query.Criteria.AddFilter(filter);
            return this;
        }

        public QueryExpressionBuilder Or(Action<QueryExpressionBuilder> nested)
        {
            var filter = new FilterExpression(LogicalOperator.Or);
            var nestedBuilder = new QueryExpressionBuilder(_query.EntityName);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._query.Criteria.Conditions)
                filter.Conditions.Add(cond);
            _query.Criteria.AddFilter(filter);
            return this;
        }

        public QueryExpressionBuilder OrderBy(string attribute, OrderType orderType = OrderType.Ascending)
        {
            _query.AddOrder(attribute, orderType);
            return this;
        }

        public QueryExpressionBuilder Top(int count)
        {
            _query.TopCount = count;
            return this;
        }

        public QueryExpressionBuilder Link(
            string toEntity,
            string fromAttribute,
            string toAttribute,
            JoinOperator joinType,
            Action<LinkEntityBuilder> linkBuilder)
        {
            var link = new LinkEntity(_query.EntityName, toEntity, fromAttribute, toAttribute, joinType);
            var builder = new LinkEntityBuilder(link);
            linkBuilder(builder);
            _query.LinkEntities.Add(link);
            return this;
        }


        public EntityCollection Execute(IOrganizationService service)
        {
            return service.RetrieveMultiple(_query);
        }

        public EntityCollection ExecuteWithPagination(IOrganizationService service, int pageNumber, int pageSize)
        {
            var query = ((QueryExpression)_query).Clone();
            query.PageInfo = new PagingInfo
            {
                PageNumber = pageNumber,
                Count = pageSize
            };
            return service.RetrieveMultiple(query);
        }

        public EntityCollection ExecuteAllPages(IOrganizationService service)
        {
            var allResults = new EntityCollection();
            int pageNumber = 1;
            int pageSize = 5000;
            string pagingCookie = null;
            bool moreRecords;

            do
            {
                var query = _query.Clone();
                query.PageInfo = new PagingInfo
                {
                    PageNumber = pageNumber,
                    Count = pageSize,
                    PagingCookie = pagingCookie
                };

                var results = service.RetrieveMultiple(query);

                if (results.Entities.Count > 0)
                    allResults.Entities.AddRange(results.Entities);

                pagingCookie = results.PagingCookie;
                moreRecords = results.MoreRecords;
                pageNumber++;

            } while (moreRecords);

            return allResults;
        }


        public async Task<EntityCollection> ExecuteAsync(IOrganizationService service)
        {
            return await Task.Run(() => Execute(service));
        }

        public async Task<EntityCollection> ExecuteWithPaginationAsync(IOrganizationService service, int pageNumber, int pageSize)
        {
            return await Task.Run(() => ExecuteWithPagination(service, pageNumber, pageSize));
        }

        public async Task<EntityCollection> ExecuteAllPagesAsync(IOrganizationService service)
        {
            return await Task.Run(() => ExecuteAllPages(service));
        }


        public QueryExpression ToQueryExpression() => _query;

        public FetchExpression ToFetchExpression(IOrganizationService service)
        {
            var queryExpressionToFetchExpressionRequest = new QueryExpressionToFetchXmlRequest
            {
                Query = _query
            };
            var response = (QueryExpressionToFetchXmlResponse)service.Execute(queryExpressionToFetchExpressionRequest);
            return new FetchExpression(response.FetchXml);
        }
    }
}