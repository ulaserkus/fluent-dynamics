using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Moq;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Builders
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
            var builder = Query.For("account").OrderByDesc("name");
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
            var clone = original.DeepClone(); // extension method
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
        [Fact]
        public void MultipleSelect_AddsAllColumns()
        {
            // Test multiple calls to Select
            var builder = Query.For("account")
                               .Select("name")
                               .Select("revenue", "description");

            var query = builder.ToQueryExpression();
            Assert.Equal(2, query.ColumnSet.Columns.Count);
            Assert.DoesNotContain("name", query.ColumnSet.Columns);
            Assert.Contains("revenue", query.ColumnSet.Columns);
            Assert.Contains("description", query.ColumnSet.Columns);
        }

        [Fact]
        public void MultipleOrderBy_AddsMultipleOrderClauses()
        {
            var builder = Query.For("account")
                               .OrderBy("name")
                               .OrderByDesc("createdon");

            var query = builder.ToQueryExpression();
            Assert.Equal(2, query.Orders.Count);
            Assert.Equal("name", query.Orders[0].AttributeName);
            Assert.Equal(OrderType.Ascending, query.Orders[0].OrderType);
            Assert.Equal("createdon", query.Orders[1].AttributeName);
            Assert.Equal(OrderType.Descending, query.Orders[1].OrderType);
        }

        [Fact]
        public void NoLock_SetsNoLockProperty()
        {
            var builder = Query.For("account").NoLock();
            var query = builder.ToQueryExpression();
            Assert.True(query.NoLock);
        }


        [Fact]
        public void Distinct_SetsDistinctProperty()
        {
            var builder = Query.For("account").Distinct();
            var query = builder.ToQueryExpression();
            Assert.True(query.Distinct);
        }

        [Fact]
        public void NestedLinks_CreateCorrectStructure()
        {
            var builder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .As("pc")
                    .Select("fullname")
                    .Link("systemuser", "ownerid", "systemuserid", JoinOperator.Inner, l2 => l2
                        .As("owner")
                        .Select("fullname")
                    )
                );

            var query = builder.ToQueryExpression();
            Assert.Single(query.LinkEntities);
            var contactLink = query.LinkEntities[0];
            Assert.Equal("contact", contactLink.LinkToEntityName);
            Assert.Single(contactLink.LinkEntities);
            var userLink = contactLink.LinkEntities[0];
            Assert.Equal("systemuser", userLink.LinkToEntityName);
            Assert.Equal("owner", userLink.EntityAlias);
        }

        [Fact]
        public void LinkWithFilter_AppliesFilterToLink()
        {
            var builder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f.Equal("statecode", 0))
                );

            var query = builder.ToQueryExpression();
            var link = query.LinkEntities[0];
            Assert.Single(link.LinkCriteria.Conditions);
            Assert.Equal("statecode", link.LinkCriteria.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Equal, link.LinkCriteria.Conditions[0].Operator);
        }

        [Fact]
        public void UseFilterExtensions_BuildsCorrectConditions()
        {
            var builder = Query.For("account")
                .Where(f => f
                    .Equal("statecode", 0)
                    .GreaterThan("revenue", 10000)
                    .Like("name", "%Contoso%")
                    .In("statuscode", 1, 2, 3)
                    .Between("createdon", DateTime.Today.AddDays(-30), DateTime.Today)
                    .IsNotNull("telephone1")
                );

            var query = builder.ToQueryExpression();
            Assert.Equal(6, query.Criteria.Conditions.Count);

            // Verify a few specific conditions
            var conditions = query.Criteria.Conditions;
            Assert.Contains(conditions, c => c.AttributeName == "statecode" && c.Operator == ConditionOperator.Equal);
            Assert.Contains(conditions, c => c.AttributeName == "revenue" && c.Operator == ConditionOperator.GreaterThan);
            Assert.Contains(conditions, c => c.AttributeName == "telephone1" && c.Operator == ConditionOperator.NotNull);
        }

        [Fact]
        public void Execute_RetrievesMultipleRecords()
        {
            // Arrange
            var serviceMock = new Mock<IOrganizationService>();
            var entityCollection = new EntityCollection
            {
                Entities = { new Entity("account"), new Entity("account") }
            };

            serviceMock.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(entityCollection);

            var builder = Query.For("account");

            // Act
            var result = builder.RetrieveMultiple(serviceMock.Object);

            // Assert
            Assert.Equal(2, result.Entities.Count);
            serviceMock.Verify(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Once);
        }

        [Fact]
        public void ExecuteFirst_ReturnsFirstEntity()
        {
            // Arrange
            var serviceMock = new Mock<IOrganizationService>();
            var entity = new Entity("account") { Id = Guid.NewGuid() };

            serviceMock.Setup(s => s.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(new EntityCollection { Entities = { entity } });

            var builder = Query.For("account");

            // Act
            var result = builder.RetrieveMultiple(serviceMock.Object).FirstOrDefault(x => x.Id != Guid.Empty);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
        }

        [Fact]
        public void QueryHint_SetsQueryHintsProperty()
        {
            var builder = Query.For("account").QueryHint("RECOMPILE");
            var query = builder.ToQueryExpression();
            Assert.Equal("RECOMPILE", query.QueryHints);
        }

        [Fact]
        public void ForceSeek_SetsForceSeekProperty()
        {
            var builder = Query.For("account").ForceSeek("IX_AccountName");
            var query = builder.ToQueryExpression();
            Assert.Equal("IX_AccountName", query.ForceSeek);
        }

        [Fact]
        public void DebugView_ReturnsQueryStructureAsString()
        {
            var builder = Query.For("account")
                .Select("name")
                .Where(f => f.Equal("statecode", 0))
                .OrderBy("name")
                .Top(10);

            var debugView = builder.DebugView();

            Assert.NotNull(debugView);
            Assert.Contains("account", debugView);
            Assert.Contains("name", debugView);
            Assert.Contains("Top Count: 10", debugView);
        }

        [Fact]
        public void ExistsAsync_ReturnsTrueWhenResultsFound()
        {
            var serviceMock = new Mock<IOrganizationServiceAsync2>();
            serviceMock.Setup(s => s.RetrieveMultipleAsync(It.IsAny<QueryExpression>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EntityCollection { Entities = { new Entity("account") } });

            var builder = Query.For("account");
            var result = builder.ExistsAsync(serviceMock.Object).Result;

            Assert.True(result);
            serviceMock.Verify(s => s.RetrieveMultipleAsync(
                It.Is<QueryExpression>(q => q.TopCount == 1 && q.ColumnSet.Columns.Count == 0),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void ExistsAsync_ReturnsFalseWhenNoResultsFound()
        {
            var serviceMock = new Mock<IOrganizationServiceAsync2>();
            serviceMock.Setup(s => s.RetrieveMultipleAsync(It.IsAny<QueryExpression>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new EntityCollection());

            var builder = Query.For("account");
            var result = builder.ExistsAsync(serviceMock.Object).Result;

            Assert.False(result);
        }
    }
}