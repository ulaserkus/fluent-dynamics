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
    }
}
