using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests
{
    public class QueryExpressionBuilderTests
    {
        [Fact]
        public void Select_AddsColumns()
        {
            var builder = Query.For("account")
                               .Select("name", "accountnumber");

            var query = builder.ToQueryExpression();
            Assert.Contains("name", query.ColumnSet.Columns);
            Assert.Contains("accountnumber", query.ColumnSet.Columns);
        }

        [Fact]
        public void SelectAll_SetsAllColumns()
        {
            var builder = Query.For("contact").SelectAll();
            var query = builder.ToQueryExpression();
            Assert.True(query.ColumnSet.AllColumns);
        }

        [Fact]
        public void Where_BuildsCriteria()
        {
            var builder = Query.For("account")
                               .Where(f => f
                                   .Condition("statecode", ConditionOperator.Equal, 0)
                                   .Or(or => or
                                        .Condition("name", ConditionOperator.Like, "%Test%")
                                        .Condition("accountnumber", ConditionOperator.Equal, "123"))
                                );

            var query = builder.ToQueryExpression();
            Assert.Equal(1, query.Criteria.Conditions.Count);
            Assert.Single(query.Criteria.Filters);
            var orFilter = query.Criteria.Filters.First();
            Assert.Equal(LogicalOperator.Or, orFilter.FilterOperator);
            Assert.Equal(2, orFilter.Conditions.Count);
        }

        [Fact]
        public void Link_AddsLinkEntity()
        {
            var builder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .As("pc")
                    .Select("firstname", "lastname")
                    .OrderBy("firstname"));

            var query = builder.ToQueryExpression();
            Assert.Single(query.LinkEntities);
            var link = query.LinkEntities[0];
            Assert.Equal("contact", link.LinkToEntityName);
            Assert.Equal("pc", link.EntityAlias);
            Assert.Contains("firstname", link.Columns.Columns);
            Assert.Single(link.Orders);
        }

        [Fact]
        public void OrderBy_AddsOrder()
        {
            var builder = Query.For("account").OrderBy("name", OrderType.Descending);
            var query = builder.ToQueryExpression();
            Assert.Single(query.Orders);
            Assert.Equal("name", query.Orders[0].AttributeName);
            Assert.Equal(OrderType.Descending, query.Orders[0].OrderType);
        }

        [Fact]
        public void Top_SetsTopCount()
        {
            var builder = Query.For("account").Top(5);
            Assert.Equal(5, builder.ToQueryExpression().TopCount);
        }

        [Fact]
        public void Clone_ProducesIndependentCopy()
        {
            var original = Query.For("account").Select("name");
            var clone = original.Clone(); // extension method
            clone.Select("accountnumber");

            var oQuery = original.ToQueryExpression();
            var cQuery = clone.ToQueryExpression();

            Assert.DoesNotContain("accountnumber", oQuery.ColumnSet.Columns);
            Assert.Contains("accountnumber", cQuery.ColumnSet.Columns);
        }

        [Fact]
        public void RetrieveMultipleAllPages_AggregatesResults()
        {
            var serviceMock = new Mock<IOrganizationService>();

            serviceMock.SetupSequence(s => s.RetrieveMultiple(It.IsAny<QueryBase>()))
                .Returns(new EntityCollection
                {
                    Entities = { new Entity("account"), new Entity("account") },
                    MoreRecords = true,
                    PagingCookie = "cookie1"
                })
                .Returns(new EntityCollection
                {
                    Entities = { new Entity("account") },
                    MoreRecords = false
                });

            var builder = Query.For("account").Select("name");
            var all = builder.RetrieveMultipleAllPages(serviceMock.Object);

            Assert.Equal(3, all.Entities.Count);
        }

        [Fact]
        public void ToFetchExpression_ProducesFetchXml()
        {
            var serviceMock = new Mock<IOrganizationService>();

            serviceMock.Setup(s => s.Execute(It.IsAny<QueryExpressionToFetchXmlRequest>()))
                .Returns<OrganizationRequest>(req =>
                {
                    return new QueryExpressionToFetchXmlResponse
                    {
                        Results = { { "FetchXml", "<fetch><entity name='account'/></fetch>" } },
                    };
                });

            var builder = Query.For("account");
            var fetch = builder.ToFetchExpression(serviceMock.Object);

            Assert.Contains("<entity name='account'", fetch.Query);
        }
    }
}