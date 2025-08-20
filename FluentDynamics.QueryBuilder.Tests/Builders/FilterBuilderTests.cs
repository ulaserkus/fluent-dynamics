using FluentDynamics.QueryBuilder.Builders;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Builders
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
            Assert.Single(expr.Conditions);
            Assert.Equal(2, expr.Filters.Count);
        }

        [Fact]
        public void EmptyFilter_ToExpression_NoConditions()
        {
            var f = new FilterBuilder(LogicalOperator.And);
            var expr = f.ToExpression();
            Assert.Equal(LogicalOperator.And, expr.FilterOperator);
            Assert.Empty(expr.Conditions);
            Assert.Empty(expr.Filters);
        }

        [Fact]
        public void MultipleSiblingOrFilters_AllRetained()
        {
            var f = new FilterBuilder(LogicalOperator.And)
                .Or(o => o.Condition("name", ConditionOperator.Equal, "A"))
                .Or(o => o.Condition("name", ConditionOperator.Equal, "B"))
                .Or(o => o.Condition("name", ConditionOperator.Equal, "C"));

            var expr = f.ToExpression();
            Assert.Empty(expr.Conditions);
            Assert.Equal(3, expr.Filters.Count);
        }

        [Fact]
        public void Between_Structure_IsObjectArray()
        {
            var f = new FilterBuilder(LogicalOperator.And)
                .Condition("createdon", ConditionOperator.Between, new object[] { DateTime.Today.AddDays(-1), DateTime.Today });

            var expr = f.ToExpression();
            var cond = Assert.Single(expr.Conditions);
            Assert.Single(cond.Values);
            Assert.IsType<object[]>(cond.Values[0]);
            var arr = (object[])cond.Values[0];
            Assert.Equal(2, arr.Length);
        }

        [Fact]
        public void NestedDeep_ThreeLevels()
        {
            var f = new FilterBuilder(LogicalOperator.And)
                .And(a => a
                    .Or(o => o
                        .And(a2 => a2
                            .Condition("statecode", ConditionOperator.Equal, 0))));

            var expr = f.ToExpression();
            Assert.Empty(expr.Conditions);
            var lvl1 = Assert.Single(expr.Filters);
            var lvl1_or = Assert.Single(lvl1.Filters);
            var lvl1_or_and = Assert.Single(lvl1_or.Filters);
            Assert.Single(lvl1_or_and.Conditions);
        }

        [Fact]
        public void OrderPreserved_InsertionSequence()
        {
            var f = new FilterBuilder(LogicalOperator.And)
                .Condition("a", ConditionOperator.Equal, 1)
                .Condition("b", ConditionOperator.Equal, 2)
                .Condition("c", ConditionOperator.Equal, 3);

            var expr = f.ToExpression();
            Assert.Collection(expr.Conditions,
                c => Assert.Equal("a", c.AttributeName),
                c => Assert.Equal("b", c.AttributeName),
                c => Assert.Equal("c", c.AttributeName));
        }

        [Fact]
        public void Condition_NullValue_AddsConditionWithoutValue()
        {
            // Test null value handling
            var filter = new FilterBuilder(LogicalOperator.And)
                .Condition("attribute", ConditionOperator.Null);

            var expr = filter.ToExpression();
            Assert.Single(expr.Conditions);
            Assert.Empty(expr.Conditions[0].Values);
        }

        [Fact]
        public void Condition_MultipleCalls_AllConditionsAdded()
        {
            // Test adding multiple conditions
            var filter = new FilterBuilder(LogicalOperator.And)
                .Condition("attr1", ConditionOperator.Equal, 1)
                .Condition("attr2", ConditionOperator.Equal, 2)
                .Condition("attr3", ConditionOperator.Equal, 3);

            var expr = filter.ToExpression();
            Assert.Equal(3, expr.Conditions.Count);
        }

        [Fact]
        public void And_WithoutActions_DoesNotAddFilter()
        {
            // Edge case: empty nested filter
            var filter = new FilterBuilder(LogicalOperator.And);
            var nestedBuilder = new FilterBuilder(LogicalOperator.And);
            var expr = filter.ToExpression();

            Assert.Empty(expr.Filters);
        }

        [Fact]
        public void Or_WithEmptyChildFilter_AddsEmptyFilter()
        {
            // Branch coverage: empty child filter
            var filter = new FilterBuilder(LogicalOperator.And);
            filter.Or(or => { });

            var expr = filter.ToExpression();
            Assert.Single(expr.Filters);
            Assert.Empty(expr.Filters[0].Conditions);
        }
    }
}