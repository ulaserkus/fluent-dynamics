using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class DynamicsExtensionsTests
    {
        [Fact]
        public void ToList_Null_ReturnsEmpty()
        {
            EntityCollection ec = null;
            var list = ec.ToList();
            Assert.Empty(list);
        }

        [Fact]
        public void TryGet_ReturnsValueOrDefault()
        {
            var e = new Entity("account")
            {
                ["name"] = "Test Account",
                ["revenue"] = 123m
            };

            var name = e.TryGet<string>("name");
            var revenue = e.TryGet<decimal>("revenue");
            var missing = e.TryGet("missing", 42);
            var wrongType = e.TryGet("name", -1);

            Assert.Equal("Test Account", name);
            Assert.Equal(123m, revenue);
            Assert.Equal(42, missing);
            Assert.Equal(-1, wrongType);
        }

        [Fact]
        public void LinqLikeExtensions_Work()
        {
            var ec = new EntityCollection();
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "Alice" });
            ec.Entities.Add(new Entity("contact") { ["firstname"] = "Bob" });

            var first = ec.FirstOrDefault(e => (string)e["firstname"] == "Alice");
            var singleOrNull = ec.SingleOrDefault(e => (string)e["firstname"] == "Alice"); // Tek eşleşme
            var whereResult = ec.Where(e => ((string)e["firstname"]).StartsWith("B")).ToList();

            Assert.NotNull(first);
            Assert.NotNull(singleOrNull);
            Assert.Single(whereResult);
        }

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