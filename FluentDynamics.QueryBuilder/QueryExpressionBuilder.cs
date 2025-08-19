using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentDynamics.QueryBuilder
{
    /// <summary>
    /// Fluent API builder for creating and executing Dynamics 365/Dataverse QueryExpressions
    /// </summary>
    public class QueryExpressionBuilder
    {
        internal QueryExpression _query;

        /// <summary>
        /// Initializes a new instance of the QueryExpressionBuilder for the specified entity
        /// </summary>
        /// <param name="entityName">Logical name of the entity to query</param>
        internal QueryExpressionBuilder(string entityName)
        {
            _query = new QueryExpression(entityName)
            {
                ColumnSet = new ColumnSet()
            };
        }

        /// <summary>
        /// Specifies which columns to include in the query results
        /// </summary>
        /// <param name="attributes">Names of attributes to include</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder Select(params string[] attributes)
        {
            _query.ColumnSet.AddColumns(attributes);
            return this;
        }

        /// <summary>
        /// Includes all columns in the query results
        /// </summary>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder SelectAll()
        {
            _query.ColumnSet = new ColumnSet(true);
            return this;
        }

        /// <summary>
        /// Adds a query hint to influence query execution plan
        /// </summary>
        /// <param name="queryHint">The query hint to apply</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder QueryHint(string queryHint)
        {
            _query.QueryHints = queryHint;
            return this;
        }

        /// <summary>
        /// Forces the query optimizer to use a specific index seek
        /// </summary>
        /// <param name="forceSeek">The index name to use for forced seek operation</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder ForceSeek(string forceSeek)
        {
            _query.ForceSeek = forceSeek;
            return this;
        }

        /// <summary>
        /// Configures the query to return only distinct records
        /// </summary>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder Distinct()
        {
            _query.Distinct = true;
            return this;
        }

        /// <summary>
        /// Applies NOLOCK hint to the query to reduce locking overhead
        /// </summary>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder NoLock()
        {
            _query.NoLock = true;
            return this;
        }

        /// <summary>
        /// Adds a filter condition to the query
        /// </summary>
        /// <param name="attribute">The attribute to filter on</param>
        /// <param name="op">The comparison operator</param>
        /// <param name="value">The value to compare against</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder Where(string attribute, ConditionOperator op, object value = default)
        {
            if (value is null)
                _query.Criteria.AddCondition(attribute, op);
            else
                _query.Criteria.AddCondition(attribute, op, value);

            return this;
        }

        /// <summary>
        /// Adds a nested AND filter group
        /// </summary>
        /// <param name="nested">Action to configure the nested filter group</param>
        /// <param name="filterHint">Optional hint for the filter</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder And(Action<QueryExpressionBuilder> nested, string filterHint = "")
        {
            var filter = new FilterExpression(LogicalOperator.And);
            filter.FilterHint = filterHint;
            var nestedBuilder = new QueryExpressionBuilder(_query.EntityName);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._query.Criteria.Conditions)
                filter.Conditions.Add(cond);
            _query.Criteria.AddFilter(filter);
            return this;
        }

        /// <summary>
        /// Adds a nested OR filter group
        /// </summary>
        /// <param name="nested">Action to configure the nested filter group</param>
        /// <param name="filterHint">Optional hint for the filter</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder Or(Action<QueryExpressionBuilder> nested, string filterHint = "")
        {
            var filter = new FilterExpression(LogicalOperator.Or);
            filter.FilterHint = filterHint;
            var nestedBuilder = new QueryExpressionBuilder(_query.EntityName);
            nested(nestedBuilder);
            foreach (var cond in nestedBuilder._query.Criteria.Conditions)
                filter.Conditions.Add(cond);
            _query.Criteria.AddFilter(filter);
            return this;
        }

        /// <summary>
        /// Adds a sort order to the query
        /// </summary>
        /// <param name="attribute">The attribute to sort on</param>
        /// <param name="orderType">The sort direction (Ascending or Descending)</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder OrderBy(string attribute, OrderType orderType = OrderType.Ascending)
        {
            _query.AddOrder(attribute, orderType);
            return this;
        }

        /// <summary>
        /// Limits the number of records returned
        /// </summary>
        /// <param name="count">Maximum number of records to return</param>
        /// <returns>The builder instance for method chaining</returns>
        public QueryExpressionBuilder Top(int count)
        {
            _query.TopCount = count;
            return this;
        }

        /// <summary>
        /// Adds a link-entity (join) to the query
        /// </summary>
        /// <param name="toEntity">The target entity to join</param>
        /// <param name="fromAttribute">The attribute from the base entity</param>
        /// <param name="toAttribute">The attribute from the target entity</param>
        /// <param name="joinType">The type of join (Inner, Outer, etc.)</param>
        /// <param name="linkBuilder">Action to configure the link entity</param>
        /// <returns>The builder instance for method chaining</returns>
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

        /// <summary>
        /// Executes the query against the organization service
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <returns>Collection of entities matching the query criteria</returns>
        public EntityCollection RetrieveMultiple(IOrganizationService service)
        {
            return service.RetrieveMultiple(_query);
        }

        /// <summary>
        /// Executes the query with pagination
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="pageNumber">The page number to retrieve</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <returns>Collection of entities for the specified page</returns>
        public EntityCollection RetrieveMultiple(IOrganizationService service, int pageNumber, int pageSize)
        {
            var query = ((QueryExpression)_query).Clone();
            query.PageInfo = new PagingInfo
            {
                PageNumber = pageNumber,
                Count = pageSize
            };
            return service.RetrieveMultiple(query);
        }

        /// <summary>
        /// Executes the query and automatically retrieves all pages of results
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <returns>Collection containing all entities from all pages</returns>
        public EntityCollection RetrieveMultipleAllPages(IOrganizationService service)
        {
            var allResults = new EntityCollection();
            int pageNumber = 1;
            int pageSize = 5000;
            string pagingCookie = null;
            bool moreRecords;
            var query = _query.Clone();

            do
            {
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

        /// <summary>
        /// Asynchronously executes the query
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Task returning collection of entities</returns>
        public Task<EntityCollection> RetrieveMultipleAsync(IOrganizationService service, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => RetrieveMultiple(service), cancellationToken);
        }

        /// <summary>
        /// Asynchronously executes the query with pagination
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="pageNumber">The page number to retrieve</param>
        /// <param name="pageSize">Number of records per page</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Task returning collection of entities for the specified page</returns>
        public Task<EntityCollection> RetrieveMultipleAsync(IOrganizationService service, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => RetrieveMultiple(service, pageNumber, pageSize), cancellationToken);
        }

        /// <summary>
        /// Asynchronously executes the query and retrieves all pages of results
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
        /// <returns>Task returning collection of all entities from all pages</returns>
        public Task<EntityCollection> RetrieveMultipleAllPagesAsync(IOrganizationService service, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => RetrieveMultipleAllPages(service), cancellationToken);
        }

        /// <summary>
        /// Converts to the underlying QueryExpression
        /// </summary>
        /// <returns>The built QueryExpression</returns>
        public QueryExpression ToQueryExpression() => _query;

        /// <summary>
        /// Converts the QueryExpression to FetchXML format
        /// </summary>
        /// <param name="service">The organization service</param>
        /// <returns>A FetchExpression containing the FetchXML equivalent of this query</returns>
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