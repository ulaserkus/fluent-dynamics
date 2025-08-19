using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests
{
    public class FilterBuilderTests
    {
        [Fact]
        public void And_Or_Nesting_Works()
        {
            var fb = new FilterBuilder(LogicalOperator.And)
                .Condition("statecode", ConditionOperator.Equal, 0)
                .Or(or => or
                    .Condition("name", ConditionOperator.Like, "%X%")
                    .Condition("accountnumber", ConditionOperator.Equal, "A1"))
                .And(and => and
                    .Condition("revenue", ConditionOperator.GreaterThan, 1000));

            var expr = fb.ToExpression();

            Assert.Equal(LogicalOperator.And, expr.FilterOperator);
            // Root: 1 direct condition (statecode) + 2 nested filters (OR, AND)
            Assert.Single(expr.Conditions);
            Assert.Equal(2, expr.Filters.Count);
        }
    }
}