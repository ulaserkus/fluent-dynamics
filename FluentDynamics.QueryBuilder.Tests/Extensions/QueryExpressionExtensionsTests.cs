using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class QueryExpressionExtensionsTests
    {
        [Fact]
        public void DeepClone_AllProperties_ArePreserved()
        {
            // Arrange
            var originalQuery = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name", "accountnumber"),
                TopCount = 100,
                Distinct = true,
                NoLock = true,
                PageInfo = new PagingInfo { Count = 50, PageNumber = 2, PagingCookie = "test" },
                QueryHints = "FORCE ORDER",
                ForceSeek = "IX_Account_Name"
            };
            originalQuery.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            originalQuery.Orders.Add(new OrderExpression("name", OrderType.Ascending));

            // Act
            var clonedQuery = originalQuery.DeepClone();

            // Assert - Verify all primitive properties are copied
            Assert.Equal(originalQuery.EntityName, clonedQuery.EntityName);
            Assert.Equal(originalQuery.TopCount, clonedQuery.TopCount);
            Assert.Equal(originalQuery.Distinct, clonedQuery.Distinct);
            Assert.Equal(originalQuery.NoLock, clonedQuery.NoLock);
            Assert.Equal(originalQuery.QueryHints, clonedQuery.QueryHints);
            Assert.Equal(originalQuery.ForceSeek, clonedQuery.ForceSeek);

            // Verify reference types are different instances but equal values
            Assert.NotSame(originalQuery.ColumnSet, clonedQuery.ColumnSet);
            Assert.NotSame(originalQuery.Criteria, clonedQuery.Criteria);
            Assert.NotSame(originalQuery.Orders, clonedQuery.Orders);
            Assert.NotSame(originalQuery.PageInfo, clonedQuery.PageInfo);

            // Verify collections have same content but different instances
            Assert.Equal(originalQuery.ColumnSet.Columns.Count, clonedQuery.ColumnSet.Columns.Count);
            Assert.Equal(originalQuery.Criteria.Conditions.Count, clonedQuery.Criteria.Conditions.Count);
            Assert.Equal(originalQuery.Orders.Count, clonedQuery.Orders.Count);
        }

        [Fact]
        public void DeepClone_WithSubQuery_CreatesIndependentCopy()
        {
            // Arrange
            var subQuery = new QueryExpression("contact")
            {
                ColumnSet = new ColumnSet("contactid")
            };
            subQuery.Criteria.AddCondition("parentcustomerid", ConditionOperator.Equal, Guid.NewGuid());

            var originalQuery = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name"),
                SubQueryExpression = subQuery
            };

            // Act
            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.SubQueryExpression.ColumnSet.AddColumns("firstname");

            // Assert
            Assert.Single(originalQuery.SubQueryExpression.ColumnSet.Columns);
            Assert.Equal(2, clonedQuery.SubQueryExpression.ColumnSet.Columns.Count);
            Assert.NotSame(originalQuery.SubQueryExpression, clonedQuery.SubQueryExpression);
            Assert.DoesNotContain("firstname", originalQuery.SubQueryExpression.ColumnSet.Columns);
            Assert.Contains("firstname", clonedQuery.SubQueryExpression.ColumnSet.Columns);
        }

        [Fact]
        public void DeepClone_NullQuery_ReturnsNull()
        {
            // Arrange
            QueryExpression nullQuery = null;

            // Act
            var result = nullQuery.DeepClone();

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void ShallowClone_LinkEntitiesContainSameReferences()
        {
            // Create original query with a link entity
            var originalQuery = new QueryExpression("account");
            var linkEntity = new LinkEntity("account", "contact", "id", "id", JoinOperator.Inner);
            originalQuery.LinkEntities.Add(linkEntity);

            // Clone the query
            var clonedQuery = originalQuery.ShallowClone();

            // The collection references are different
            Assert.NotSame(originalQuery.LinkEntities, clonedQuery.LinkEntities);

            // But the item references inside the collections are the same
            Assert.Same(linkEntity, clonedQuery.LinkEntities[0]);

            // Modifying an item in one collection affects both because they reference the same object
            linkEntity.EntityAlias = "test_alias";
            Assert.Equal("test_alias", originalQuery.LinkEntities[0].EntityAlias);
            Assert.Equal("test_alias", clonedQuery.LinkEntities[0].EntityAlias);

            // Adding to one collection doesn't affect the other
            clonedQuery.LinkEntities.Add(new LinkEntity("account", "opportunity", "id", "id", JoinOperator.Inner));
            Assert.Single(originalQuery.LinkEntities);
            Assert.Equal(2, clonedQuery.LinkEntities.Count);
        }

        [Fact]
        public void CloneForPagination_CopiesQueryPropertiesExceptPaging()
        {
            // Test pagination clone preserves original settings
            var originalQuery = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("name"),
                TopCount = 100,
                Distinct = true,
                NoLock = true
            };
            originalQuery.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            var clonedQuery = originalQuery.CloneForPagination(2, 50, "cookie");

            Assert.Equal(originalQuery.EntityName, clonedQuery.EntityName);
            Assert.Equal(originalQuery.TopCount, clonedQuery.TopCount);
            Assert.Equal(originalQuery.Distinct, clonedQuery.Distinct);
            Assert.Equal(originalQuery.NoLock, clonedQuery.NoLock);
            Assert.Equal(2, clonedQuery.PageInfo.PageNumber);
            Assert.Equal(50, clonedQuery.PageInfo.Count);
            Assert.Equal("cookie", clonedQuery.PageInfo.PagingCookie);
        }

        [Fact]
        public void CloneForPagination_NullQuery_ReturnsNull()
        {
            // Test null handling
            QueryExpression nullQuery = null;
            var result = nullQuery.CloneForPagination(1, 50);
            Assert.Null(result);
        }

        [Fact]
        public void DeepClone_NestedLinkEntityCriteria_CreatesIndependentCopy()
        {
            // Test deep cloning of nested link entity filters
            var originalQuery = new QueryExpression("account");
            var link = new LinkEntity("account", "contact", "id", "id", JoinOperator.Inner);
            link.LinkCriteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            var nestedLink = new LinkEntity("contact", "opportunity", "id", "id", JoinOperator.Inner);
            nestedLink.LinkCriteria.AddCondition("statecode", ConditionOperator.Equal, 0);
            link.LinkEntities.Add(nestedLink);
            originalQuery.LinkEntities.Add(link);

            var clonedQuery = originalQuery.DeepClone();
            clonedQuery.LinkEntities[0].LinkCriteria.AddCondition("name", ConditionOperator.NotNull);
            clonedQuery.LinkEntities[0].LinkEntities[0].LinkCriteria.AddCondition("name", ConditionOperator.NotNull);

            Assert.Single(originalQuery.LinkEntities[0].LinkCriteria.Conditions);
            Assert.Equal(2, clonedQuery.LinkEntities[0].LinkCriteria.Conditions.Count);

            Assert.Single(originalQuery.LinkEntities[0].LinkEntities[0].LinkCriteria.Conditions);
            Assert.Equal(2, clonedQuery.LinkEntities[0].LinkEntities[0].LinkCriteria.Conditions.Count);
        }
    }
}
