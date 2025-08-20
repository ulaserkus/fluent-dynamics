using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class QueryBuilderExtensionsTests
    {
        [Fact]
        public void DeepClone_ColumnSet_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account").Select("name", "accountnumber");
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.ColumnSet.AddColumns("telephone1");

            // Assert
            Assert.Equal(2, originalQuery.ColumnSet.Columns.Count);
            Assert.Equal(3, clonedQuery.ColumnSet.Columns.Count);
            Assert.DoesNotContain("telephone1", originalQuery.ColumnSet.Columns);
            Assert.Contains("telephone1", clonedQuery.ColumnSet.Columns);
        }

        [Fact]
        public void DeepClone_Criteria_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account")
                .Where(f => f.Condition("statecode", ConditionOperator.Equal, 0));
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.Criteria.AddCondition("name", ConditionOperator.NotNull);

            // Assert
            Assert.Single(originalQuery.Criteria.Conditions);
            Assert.Equal(2, clonedQuery.Criteria.Conditions.Count);
            Assert.NotSame(originalQuery.Criteria, clonedQuery.Criteria);
        }

        [Fact]
        public void DeepClone_LinkEntities_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Select("firstname", "lastname")
                    .As("contact"));
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.LinkEntities[0].Columns.AddColumns("emailaddress1");

            // Assert
            Assert.Equal(2, originalQuery.LinkEntities[0].Columns.Columns.Count);
            Assert.Equal(3, clonedQuery.LinkEntities[0].Columns.Columns.Count);
            Assert.NotSame(originalQuery.LinkEntities[0], clonedQuery.LinkEntities[0]);
            Assert.DoesNotContain("emailaddress1", originalQuery.LinkEntities[0].Columns.Columns);
            Assert.Contains("emailaddress1", clonedQuery.LinkEntities[0].Columns.Columns);
        }

        [Fact]
        public void DeepClone_Orders_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account").OrderBy("name");
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.Orders.Add(new OrderExpression("accountnumber", OrderType.Descending));

            // Assert
            Assert.Single(originalQuery.Orders);
            Assert.Equal(2, clonedQuery.Orders.Count);
            Assert.NotSame(originalQuery.Orders, clonedQuery.Orders);
            Assert.NotSame(originalQuery.Orders[0], clonedQuery.Orders[0]);
        }

        [Fact]
        public void DeepClone_NestedLinkEntities_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e
                        .Select("subject")
                        .As("em")));
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.LinkEntities[0].LinkEntities[0].Columns.AddColumns("createdon");

            // Assert
            var originalNestedLink = originalQuery.LinkEntities[0].LinkEntities[0];
            var clonedNestedLink = clonedQuery.LinkEntities[0].LinkEntities[0];

            Assert.Single(originalNestedLink.Columns.Columns);
            Assert.Equal(2, clonedNestedLink.Columns.Columns.Count);
            Assert.NotSame(originalNestedLink, clonedNestedLink);
            Assert.DoesNotContain("createdon", originalNestedLink.Columns.Columns);
            Assert.Contains("createdon", clonedNestedLink.Columns.Columns);
        }

        [Fact]
        public void DeepClone_LinkEntityCriteria_CreatesIndependentCopy()
        {
            // Arrange
            var originalBuilder = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f.Condition("statecode", ConditionOperator.Equal, 0)));
            var originalQuery = originalBuilder.ToQueryExpression();

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.LinkEntities[0].LinkCriteria.AddCondition("firstname", ConditionOperator.NotNull);

            // Assert
            var originalLinkCriteria = originalQuery.LinkEntities[0].LinkCriteria;
            var clonedLinkCriteria = clonedQuery.LinkEntities[0].LinkCriteria;

            Assert.Single(originalLinkCriteria.Conditions);
            Assert.Equal(2, clonedLinkCriteria.Conditions.Count);
            Assert.NotSame(originalLinkCriteria, clonedLinkCriteria);
        }

        [Fact]
        public void ShallowClone_ProducesIndependentQueryBuilderButSharedCollections()
        {
            var original = Query.For("account").Select("name");
            var clone = original.ShallowClone();

            // Modify clone's query
            clone.ToQueryExpression().TopCount = 10;

            // Verify independence of main properties
            Assert.Null(original.ToQueryExpression().TopCount);
            Assert.Equal(10, clone.ToQueryExpression().TopCount);

            // But collections like ColumnSet are shared
            clone.ToQueryExpression().ColumnSet.AddColumns("telephone1");
            Assert.Contains("telephone1", original.ToQueryExpression().ColumnSet.Columns);
        }

        [Fact]
        public void CloneForPagination_SetsPageInfoCorrectly()
        {
            var original = Query.For("account").Select("name");
            var clone = original.CloneForPagination(3, 50, "testcookie");

            var query = clone.ToQueryExpression();
            Assert.Equal(3, query.PageInfo.PageNumber);
            Assert.Equal(50, query.PageInfo.Count);
            Assert.Equal("testcookie", query.PageInfo.PagingCookie);
        }

        [Fact]
        public void DebugView_ComplexQuery_IncludesAllElements()
        {
            var builder = Query.For("account")
                .Select("name", "accountnumber")
                .Where(f => f
                    .Equal("statecode", 0)
                    .And(a => a.Equal("name", "Test")))
                .OrderBy("name")
                .Top(10)
                .NoLock()
                .Distinct()
                .Link("contact", "accountid", "parentcustomerid", JoinOperator.Inner, l => l
                    .Select("fullname")
                    .Where(f => f.Equal("statecode", 0))
                    .OrderBy("fullname"));

            var debugView = builder.DebugView();

            // Check that debug view contains important elements
            Assert.Contains("QueryExpression: account", debugView);
            Assert.Contains("name", debugView);
            Assert.Contains("accountnumber", debugView);
            Assert.Contains("Criteria:", debugView);
            Assert.Contains("Link Entities:", debugView);
            Assert.Contains("Order By:", debugView);
            Assert.Contains("Top Count: 10", debugView);
            Assert.Contains("NoLock: Yes", debugView);
            Assert.Contains("Distinct: Yes", debugView);
        }

        [Fact]
        public void DebugView_FormatValues_HandlesSpecialTypes()
        {
            // This test checks that the DebugView correctly formats special types
            var id = Guid.NewGuid();
            var builder = Query.For("account")
                .Where(f => f
                    .Equal("accountid", id)
                    .Equal("createdon", new DateTime(2023, 1, 1)));

            var debugView = builder.DebugView();

            Assert.Contains(id.ToString("B"), debugView);
            Assert.Contains("2023-01-01", debugView);
        }
    }
}
