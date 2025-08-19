using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests
{
    public class QueryTests
    {
        [Fact]
        public void For_ReturnsBuilderWithCorrectEntityName()
        {
            var builder = Query.For("account");
            var qx = builder.ToQueryExpression();

            Assert.Equal("account", qx.EntityName);
            Assert.False(qx.ColumnSet.AllColumns);
            Assert.Empty(qx.ColumnSet.Columns);
        }

        [Fact]
        public void For_CalledTwice_ReturnsDistinctBuilderInstances()
        {
            var b1 = Query.For("account").Select("name");
            var b2 = Query.For("account").Select("accountnumber");

            var q1 = b1.ToQueryExpression();
            var q2 = b2.ToQueryExpression();

            Assert.Contains("name", q1.ColumnSet.Columns);
            Assert.DoesNotContain("accountnumber", q1.ColumnSet.Columns);

            Assert.Contains("accountnumber", q2.ColumnSet.Columns);
            Assert.DoesNotContain("name", q2.ColumnSet.Columns);
        }

        [Fact]
        public void For_DifferentEntities_AreIndependent()
        {
            var accountBuilder = Query.For("account").Select("name");
            var contactBuilder = Query.For("contact").Select("firstname");

            var accountQuery = accountBuilder.ToQueryExpression();
            var contactQuery = contactBuilder.ToQueryExpression();

            Assert.Equal("account", accountQuery.EntityName);
            Assert.Equal("contact", contactQuery.EntityName);

            Assert.Contains("name", accountQuery.ColumnSet.Columns);
            Assert.DoesNotContain("firstname", accountQuery.ColumnSet.Columns);

            Assert.Contains("firstname", contactQuery.ColumnSet.Columns);
            Assert.DoesNotContain("name", contactQuery.ColumnSet.Columns);
        }

        [Fact]
        public void For_BuilderChaining_WorksAsExpected()
        {
            var builder = Query.For("account")
                               .Select("name", "accountnumber")
                               .Where(f => f
                                    .Condition("statecode", ConditionOperator.Equal, 0)
                                    .And(and => and.Condition("name", ConditionOperator.NotNull))
                                )
                               .OrderBy("name")
                               .Top(10);

            var qx = builder.ToQueryExpression();

            Assert.Equal("account", qx.EntityName);
            Assert.Contains("name", qx.ColumnSet.Columns);
            Assert.Contains("accountnumber", qx.ColumnSet.Columns);

            Assert.Equal(LogicalOperator.And, qx.Criteria.FilterOperator);
            Assert.Single(qx.Criteria.Conditions);
            Assert.Single(qx.Criteria.Filters);

            var nested = qx.Criteria.Filters[0];
            Assert.Equal(LogicalOperator.And, nested.FilterOperator);
            Assert.Single(nested.Conditions);

            Assert.Single(qx.Orders);
            Assert.Equal("name", qx.Orders[0].AttributeName);
            Assert.Equal(OrderType.Ascending, qx.Orders[0].OrderType);

            Assert.Equal(10, qx.TopCount);
        }

        [Fact]
        public void For_SelectAll_SetsAllColumns()
        {
            var builder = Query.For("account").SelectAll();
            var qx = builder.ToQueryExpression();

            Assert.True(qx.ColumnSet.AllColumns);
            Assert.Empty(qx.ColumnSet.Columns);
        }

        [Fact]
        public void For_ChainingAfterSelectAll_OverridesToSpecificColumns()
        {
            var builder = Query.For("account")
                               .SelectAll()
                               .Select("name", "accountnumber");

            var qx = builder.ToQueryExpression();
            Assert.False(qx.ColumnSet.AllColumns);
            Assert.Contains("name", qx.ColumnSet.Columns);
            Assert.Contains("accountnumber", qx.ColumnSet.Columns);
        }

        [Fact]
        public void For_Top_OverridesPreviousTop()
        {
            var builder = Query.For("account")
                .Top(5)
                .Top(20);

            var qx = builder.ToQueryExpression();
            Assert.Equal(20, qx.TopCount);
        }
    }
}