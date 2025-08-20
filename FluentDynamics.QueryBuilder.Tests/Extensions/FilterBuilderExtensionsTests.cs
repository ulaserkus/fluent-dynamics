using FluentDynamics.QueryBuilder.Builders;
using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Extensions
{
    public class FilterBuilderExtensionsTests
    {
        #region Basic Comparison Operators Tests

        [Fact]
        public void Equal_ShouldAddEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Equal("name", "test");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Equal, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("test", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NotEqual_ShouldAddNotEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotEqual("statecode", 1);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("statecode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotEqual, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(1, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void GreaterThan_ShouldAddGreaterThanCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.GreaterThan("revenue", 10000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.GreaterThan, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(10000, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LessThan_ShouldAddLessThanCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LessThan("revenue", 10000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LessThan, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(10000, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void GreaterEqual_ShouldAddGreaterEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.GreaterEqual("revenue", 10000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.GreaterEqual, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(10000, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LessEqual_ShouldAddLessEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LessEqual("revenue", 10000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LessEqual, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(10000, expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Null Operators Tests

        [Fact]
        public void IsNull_ShouldAddNullCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.IsNull("parentcustomerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Null, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void IsNotNull_ShouldAddNotNullCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.IsNotNull("emailaddress1");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("emailaddress1", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotNull, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        #endregion

        #region String Comparison Operators Tests

        [Fact]
        public void Like_ShouldAddLikeCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Like("name", "%test%");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Like, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("%test%", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NotLike_ShouldAddNotLikeCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotLike("name", "%test%");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotLike, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("%test%", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void BeginsWith_ShouldAddBeginsWithCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.BeginsWith("name", "Contoso");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.BeginsWith, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("Contoso", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void DoesNotBeginWith_ShouldAddDoesNotBeginWithCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.DoesNotBeginWith("name", "Contoso");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.DoesNotBeginWith, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("Contoso", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void EndsWith_ShouldAddEndsWithCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EndsWith("name", "Inc");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EndsWith, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("Inc", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void DoesNotEndWith_ShouldAddDoesNotEndWithCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.DoesNotEndWith("name", "Inc");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("name", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.DoesNotEndWith, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("Inc", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void Contains_ShouldAddContainsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Contains("description", "important");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("description", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Contains, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("important", expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void DoesNotContain_ShouldAddDoesNotContainCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.DoesNotContain("description", "canceled");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("description", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContain, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal("canceled", expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Collection Operators Tests

        [Fact]
        public void In_ShouldAddInCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.In("statuscode", 1, 2, 3);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("statuscode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.In, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(3, valuesArray.Length);
            Assert.Equal(1, valuesArray[0]);
            Assert.Equal(2, valuesArray[1]);
            Assert.Equal(3, valuesArray[2]);
        }

        [Fact]
        public void NotIn_ShouldAddNotInCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotIn("statuscode", 1, 2, 3);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("statuscode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotIn, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(3, valuesArray.Length);
            Assert.Equal(1, valuesArray[0]);
            Assert.Equal(2, valuesArray[1]);
            Assert.Equal(3, valuesArray[2]);
        }

        [Fact]
        public void Between_ShouldAddBetweenCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Between("revenue", 10000, 50000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Between, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(10000, valuesArray[0]);
            Assert.Equal(50000, valuesArray[1]);
        }

        [Fact]
        public void NotBetween_ShouldAddNotBetweenCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotBetween("revenue", 10000, 50000);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("revenue", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotBetween, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(10000, valuesArray[0]);
            Assert.Equal(50000, valuesArray[1]);
        }

        [Fact]
        public void ContainValues_ShouldAddContainValuesCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ContainValues("categorycode", 1, 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("categorycode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ContainValues, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an int array containing the actual values
            Assert.IsType<int[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (int[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(1, valuesArray[0]);
            Assert.Equal(2, valuesArray[1]);
        }

        [Fact]
        public void DoesNotContainValues_ShouldAddDoesNotContainValuesCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.DoesNotContainValues("categorycode", 1, 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("categorycode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.DoesNotContainValues, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an int array containing the actual values
            Assert.IsType<int[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (int[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(1, valuesArray[0]);
            Assert.Equal(2, valuesArray[1]);
        }

        #endregion

        #region Date Operators - Absolute Tests

        [Fact]
        public void On_ShouldAddOnCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var date = new DateTime(2023, 1, 1);

            // Act
            filter.On("createdon", date);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.On, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(date, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NotOn_ShouldAddNotOnCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var date = new DateTime(2023, 1, 1);

            // Act
            filter.NotOn("createdon", date);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotOn, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(date, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OnOrBefore_ShouldAddOnOrBeforeCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var date = new DateTime(2023, 1, 1);

            // Act
            filter.OnOrBefore("createdon", date);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OnOrBefore, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(date, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OnOrAfter_ShouldAddOnOrAfterCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var date = new DateTime(2023, 1, 1);

            // Act
            filter.OnOrAfter("createdon", date);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OnOrAfter, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(date, expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Date Operators - Relative (No Parameters) Tests

        [Fact]
        public void Yesterday_ShouldAddYesterdayCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Yesterday("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Yesterday, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void Today_ShouldAddTodayCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Today("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Today, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void Tomorrow_ShouldAddTomorrowCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Tomorrow("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Tomorrow, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void Last7Days_ShouldAddLast7DaysCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Last7Days("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Last7Days, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void Next7Days_ShouldAddNext7DaysCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Next7Days("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Next7Days, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void LastWeek_ShouldAddLastWeekCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastWeek("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastWeek, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void ThisWeek_ShouldAddThisWeekCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ThisWeek("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ThisWeek, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NextWeek_ShouldAddNextWeekCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextWeek("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextWeek, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void LastMonth_ShouldAddLastMonthCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastMonth("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastMonth, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void ThisMonth_ShouldAddThisMonthCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ThisMonth("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ThisMonth, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NextMonth_ShouldAddNextMonthCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextMonth("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextMonth, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void LastYear_ShouldAddLastYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void ThisYear_ShouldAddThisYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ThisYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ThisYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NextYear_ShouldAddNextYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        #endregion

        #region Date Operators - Relative (With Parameters) Tests

        [Fact]
        public void LastXHours_ShouldAddLastXHoursCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXHours("createdon", 24);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXHours, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(24, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXHours_ShouldAddNextXHoursCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXHours("createdon", 24);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXHours, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(24, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LastXDays_ShouldAddLastXDaysCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXDays("createdon", 7);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXDays, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(7, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXDays_ShouldAddNextXDaysCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXDays("createdon", 7);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXDays, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(7, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LastXWeeks_ShouldAddLastXWeeksCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXWeeks("createdon", 4);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXWeeks, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(4, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXWeeks_ShouldAddNextXWeeksCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXWeeks("createdon", 4);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXWeeks, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(4, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LastXMonths_ShouldAddLastXMonthsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXMonths("createdon", 6);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXMonths, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(6, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXMonths_ShouldAddNextXMonthsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXMonths("createdon", 6);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXMonths, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(6, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LastXYears_ShouldAddLastXYearsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXYears("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXYears, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXYears_ShouldAddNextXYearsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXYears("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXYears, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXMinutes_ShouldAddOlderThanXMinutesCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXMinutes("createdon", 30);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMinutes, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(30, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXHours_ShouldAddOlderThanXHoursCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXHours("createdon", 24);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXHours, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(24, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXDays_ShouldAddOlderThanXDaysCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXDays("createdon", 7);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXDays, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(7, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXWeeks_ShouldAddOlderThanXWeeksCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXWeeks("createdon", 4);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXWeeks, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(4, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXMonths_ShouldAddOlderThanXMonthsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXMonths("createdon", 6);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXMonths, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(6, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void OlderThanXYears_ShouldAddOlderThanXYearsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.OlderThanXYears("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.OlderThanXYears, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Fiscal Period Operators - No Parameters Tests

        [Fact]
        public void ThisFiscalYear_ShouldAddThisFiscalYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ThisFiscalYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void ThisFiscalPeriod_ShouldAddThisFiscalPeriodCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.ThisFiscalPeriod("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.ThisFiscalPeriod, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NextFiscalYear_ShouldAddNextFiscalYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextFiscalYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NextFiscalPeriod_ShouldAddNextFiscalPeriodCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextFiscalPeriod("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextFiscalPeriod, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void LastFiscalYear_ShouldAddLastFiscalYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastFiscalYear("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalYear, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void LastFiscalPeriod_ShouldAddLastFiscalPeriodCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastFiscalPeriod("createdon");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastFiscalPeriod, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        #endregion

        #region Fiscal Period Operators - With Parameters Tests

        [Fact]
        public void LastXFiscalYears_ShouldAddLastXFiscalYearsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXFiscalYears("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalYears, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void LastXFiscalPeriods_ShouldAddLastXFiscalPeriodsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.LastXFiscalPeriods("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.LastXFiscalPeriods, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXFiscalYears_ShouldAddNextXFiscalYearsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXFiscalYears("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalYears, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NextXFiscalPeriods_ShouldAddNextXFiscalPeriodsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NextXFiscalPeriods("createdon", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NextXFiscalPeriods, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void InFiscalYear_ShouldAddInFiscalYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.InFiscalYear("createdon", 2023);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.InFiscalYear, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2023, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void InFiscalPeriod_ShouldAddInFiscalPeriodCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.InFiscalPeriod("createdon", 3);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriod, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(3, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void InFiscalPeriodAndYear_ShouldAddInFiscalPeriodAndYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.InFiscalPeriodAndYear("createdon", 3, 2023);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.InFiscalPeriodAndYear, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(3, valuesArray[0]);
            Assert.Equal(2023, valuesArray[1]);
        }

        [Fact]
        public void InOrBeforeFiscalPeriodAndYear_ShouldAddInOrBeforeFiscalPeriodAndYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.InOrBeforeFiscalPeriodAndYear("createdon", 3, 2023);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.InOrBeforeFiscalPeriodAndYear, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(3, valuesArray[0]);
            Assert.Equal(2023, valuesArray[1]);
        }

        [Fact]
        public void InOrAfterFiscalPeriodAndYear_ShouldAddInOrAfterFiscalPeriodAndYearCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.InOrAfterFiscalPeriodAndYear("createdon", 3, 2023);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("createdon", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.InOrAfterFiscalPeriodAndYear, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);

            // Check that Values[0] is an object array containing the actual values
            Assert.IsType<object[]>(expression.Conditions[0].Values[0]);
            var valuesArray = (object[])expression.Conditions[0].Values[0];
            Assert.Equal(2, valuesArray.Length);
            Assert.Equal(3, valuesArray[0]);
            Assert.Equal(2023, valuesArray[1]);
        }

        #endregion

        #region User and Business Operators Tests

        [Fact]
        public void EqualUserId_ShouldAddEqualUserIdCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserId("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserId, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NotEqualUserId_ShouldAddNotEqualUserIdCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotEqualUserId("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotEqualUserId, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualUserTeams_ShouldAddEqualUserTeamsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserTeams("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserTeams, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualUserOrUserTeams_ShouldAddEqualUserOrUserTeamsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserOrUserTeams("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserTeams, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualUserOrUserHierarchy_ShouldAddEqualUserOrUserHierarchyCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserOrUserHierarchy("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualUserOrUserHierarchyAndTeams_ShouldAddEqualUserOrUserHierarchyAndTeamsCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserOrUserHierarchyAndTeams("ownerid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("ownerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchyAndTeams, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualBusinessId_ShouldAddEqualBusinessIdCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualBusinessId("businessunitid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("businessunitid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualBusinessId, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void NotEqualBusinessId_ShouldAddNotEqualBusinessIdCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotEqualBusinessId("businessunitid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("businessunitid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotEqualBusinessId, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        [Fact]
        public void EqualUserLanguage_ShouldAddEqualUserLanguageCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.EqualUserLanguage("languageid");

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("languageid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserLanguage, expression.Conditions[0].Operator);
            Assert.Empty(expression.Conditions[0].Values);
        }

        #endregion

        #region Hierarchy Operators Tests

        [Fact]
        public void Under_ShouldAddUnderCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var recordId = Guid.NewGuid();

            // Act
            filter.Under("parentcustomerid", recordId);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Under, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(recordId, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NotUnder_ShouldAddNotUnderCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var recordId = Guid.NewGuid();

            // Act
            filter.NotUnder("parentcustomerid", recordId);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotUnder, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(recordId, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void UnderOrEqual_ShouldAddUnderOrEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var recordId = Guid.NewGuid();

            // Act
            filter.UnderOrEqual("parentcustomerid", recordId);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.UnderOrEqual, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(recordId, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void Above_ShouldAddAboveCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var recordId = Guid.NewGuid();

            // Act
            filter.Above("parentcustomerid", recordId);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Above, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(recordId, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void AboveOrEqual_ShouldAddAboveOrEqualCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var recordId = Guid.NewGuid();

            // Act
            filter.AboveOrEqual("parentcustomerid", recordId);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("parentcustomerid", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.AboveOrEqual, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(recordId, expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Bit Mask Operators Tests

        [Fact]
        public void Mask_ShouldAddMaskCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Mask("attributes", 1);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("attributes", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Mask, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(1, expression.Conditions[0].Values[0]);
        }

        [Fact]
        public void NotMask_ShouldAddNotMaskCondition()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.NotMask("attributes", 2);

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions);
            Assert.Equal("attributes", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.NotMask, expression.Conditions[0].Operator);
            Assert.Single(expression.Conditions[0].Values);
            Assert.Equal(2, expression.Conditions[0].Values[0]);
        }

        #endregion

        #region Complex Filter Tests

        [Fact]
        public void ChainedConditions_ShouldBuildComplexFilter()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Equal("statecode", 0)
                  .GreaterThan("revenue", 10000)
                  .IsNotNull("telephone1");

            // Assert
            var expression = filter.ToExpression();
            Assert.Equal(3, expression.Conditions.Count);
            Assert.Equal(LogicalOperator.And, expression.FilterOperator);

            // Check each condition
            Assert.Equal("statecode", expression.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Equal, expression.Conditions[0].Operator);
            Assert.Equal(0, expression.Conditions[0].Values[0]);

            Assert.Equal("revenue", expression.Conditions[1].AttributeName);
            Assert.Equal(ConditionOperator.GreaterThan, expression.Conditions[1].Operator);
            Assert.Equal(10000, expression.Conditions[1].Values[0]);

            Assert.Equal("telephone1", expression.Conditions[2].AttributeName);
            Assert.Equal(ConditionOperator.NotNull, expression.Conditions[2].Operator);
        }

        [Fact]
        public void NestedFilters_ShouldBuildComplexFilter()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Equal("statecode", 0)
                  .And(f => f
                      .GreaterThan("revenue", 10000)
                      .Today("createdon"))
                  .Or(f => f
                      .Equal("prioritycode", 1)
                      .LastXDays("modifiedon", 7));

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions); // statecode = 0
            Assert.Equal(2, expression.Filters.Count); // AND and OR filters

            // Verify AND filter
            var andFilter = expression.Filters[0];
            Assert.Equal(LogicalOperator.And, andFilter.FilterOperator);
            Assert.Equal(2, andFilter.Conditions.Count); // revenue > 10000, createdon = today

            Assert.Equal("revenue", andFilter.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.GreaterThan, andFilter.Conditions[0].Operator);
            Assert.Equal(10000, andFilter.Conditions[0].Values[0]);

            Assert.Equal("createdon", andFilter.Conditions[1].AttributeName);
            Assert.Equal(ConditionOperator.Today, andFilter.Conditions[1].Operator);
            Assert.Empty(andFilter.Conditions[1].Values);

            // Verify OR filter
            var orFilter = expression.Filters[1];
            Assert.Equal(LogicalOperator.Or, orFilter.FilterOperator);
            Assert.Equal(2, orFilter.Conditions.Count); // prioritycode = 1, modifiedon in last 7 days

            Assert.Equal("prioritycode", orFilter.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Equal, orFilter.Conditions[0].Operator);
            Assert.Equal(1, orFilter.Conditions[0].Values[0]);

            Assert.Equal("modifiedon", orFilter.Conditions[1].AttributeName);
            Assert.Equal(ConditionOperator.LastXDays, orFilter.Conditions[1].Operator);
            Assert.Equal(7, orFilter.Conditions[1].Values[0]);
        }

        [Fact]
        public void MultipleNestedFilters_ShouldBuildComplexFilter()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);

            // Act
            filter.Equal("statecode", 0)
                  .And(f => f
                      .GreaterThan("revenue", 10000)
                      .And(f2 => f2
                          .Like("name", "%Inc%")
                          .BeginsWith("address1_city", "New")));

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions); // statecode = 0
            Assert.Single(expression.Filters); // AND filter

            // Verify first-level AND filter
            var andFilter = expression.Filters[0];
            Assert.Equal(LogicalOperator.And, andFilter.FilterOperator);
            Assert.Single(andFilter.Conditions); // revenue > 10000
            Assert.Single(andFilter.Filters); // nested AND filter

            // Verify second-level AND filter
            var nestedAndFilter = andFilter.Filters[0];
            Assert.Equal(LogicalOperator.And, nestedAndFilter.FilterOperator);
            Assert.Equal(2, nestedAndFilter.Conditions.Count); // name LIKE %Inc%, address1_city BEGINSWITH New

            Assert.Equal("name", nestedAndFilter.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Like, nestedAndFilter.Conditions[0].Operator);
            Assert.Equal("%Inc%", nestedAndFilter.Conditions[0].Values[0]);

            Assert.Equal("address1_city", nestedAndFilter.Conditions[1].AttributeName);
            Assert.Equal(ConditionOperator.BeginsWith, nestedAndFilter.Conditions[1].Operator);
            Assert.Equal("New", nestedAndFilter.Conditions[1].Values[0]);
        }

        [Fact]
        public void ComplexDateFilter_ShouldBuildCorrectDateConditions()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var today = DateTime.Today;

            // Act
            filter.Equal("statecode", 0)
                  .Or(f => f
                      .OnOrAfter("createdon", today.AddDays(-30))
                      .And(f2 => f2
                          .OlderThanXDays("createdon", 3)
                          .NextXMonths("scheduledend", 2))
                      .ThisFiscalYear("createdon"));

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions); // statecode = 0
            Assert.Single(expression.Filters); // OR filter

            // Verify OR filter
            var orFilter = expression.Filters[0];
            Assert.Equal(LogicalOperator.Or, orFilter.FilterOperator);
            Assert.Equal(2, orFilter.Conditions.Count); // OnOrAfter and ThisFiscalYear
            Assert.Single(orFilter.Filters); // AND filter

            // Verify the AND filter within OR
            var andFilter = orFilter.Filters[0];
            Assert.Equal(LogicalOperator.And, andFilter.FilterOperator);
            Assert.Equal(2, andFilter.Conditions.Count); // OlderThanXDays and NextXMonths
        }

        [Fact]
        public void ComplexHierarchyFilter_ShouldBuildCorrectConditions()
        {
            // Arrange
            var filter = new FilterBuilder(LogicalOperator.And);
            var parentId = Guid.NewGuid();
            var businessId = Guid.NewGuid();

            // Act
            filter.Equal("statecode", 0)
                  .And(f => f
                      .UnderOrEqual("parentcustomerid", parentId)
                      .Or(f2 => f2
                          .Under("parentcustomerid", businessId)
                          .EqualUserOrUserHierarchy("ownerid")));

            // Assert
            var expression = filter.ToExpression();
            Assert.Single(expression.Conditions); // statecode = 0
            Assert.Single(expression.Filters); // AND filter

            // Verify AND filter
            var andFilter = expression.Filters[0];
            Assert.Equal(LogicalOperator.And, andFilter.FilterOperator);
            Assert.Single(andFilter.Conditions); // UnderOrEqual
            Assert.Single(andFilter.Filters); // OR filter

            // Verify OR filter
            var orFilter = andFilter.Filters[0];
            Assert.Equal(LogicalOperator.Or, orFilter.FilterOperator);
            Assert.Equal(2, orFilter.Conditions.Count); // Under and EqualUserOrUserHierarchy

            // Check specific conditions
            Assert.Equal("parentcustomerid", andFilter.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.UnderOrEqual, andFilter.Conditions[0].Operator);
            Assert.Equal(parentId, andFilter.Conditions[0].Values[0]);

            Assert.Equal("parentcustomerid", orFilter.Conditions[0].AttributeName);
            Assert.Equal(ConditionOperator.Under, orFilter.Conditions[0].Operator);
            Assert.Equal(businessId, orFilter.Conditions[0].Values[0]);

            Assert.Equal("ownerid", orFilter.Conditions[1].AttributeName);
            Assert.Equal(ConditionOperator.EqualUserOrUserHierarchy, orFilter.Conditions[1].Operator);
            Assert.Empty(orFilter.Conditions[1].Values);
        }

        #endregion

        [Fact]
        public void ChainedFilterExtensions_BuildsCorrectFilterExpression()
        {
            // Test chaining multiple extension methods
            var filter = new FilterBuilder(LogicalOperator.And)
                .Equal("statecode", 0)
                .IsNotNull("name")
                .GreaterThan("revenue", 10000);

            var expr = filter.ToExpression();
            Assert.Equal(3, expr.Conditions.Count);
            Assert.Equal(LogicalOperator.And, expr.FilterOperator);
        }

        [Fact]
        public void NestedFilterExtensions_BuildsCorrectFilterHierarchy()
        {
            // Test nesting extensions with And/Or
            var filter = new FilterBuilder(LogicalOperator.And)
                .Equal("statecode", 0)
                .And(a => a
                    .GreaterThan("revenue", 10000)
                    .LastXDays("createdon", 30)
                )
                .Or(o => o
                    .Like("name", "%Inc%")
                    .EndsWith("name", "LLC"));

            var expr = filter.ToExpression();
            Assert.Single(expr.Conditions);
            Assert.Equal(2, expr.Filters.Count);

            var andFilter = expr.Filters[0];
            Assert.Equal(2, andFilter.Conditions.Count);

            var orFilter = expr.Filters[1];
            Assert.Equal(2, orFilter.Conditions.Count);
        }

        [Fact]
        public void DateTimeExtensions_HandlesTimeComponent()
        {
            // Test date/time handling in extensions
            var date = new DateTime(2023, 1, 1, 14, 30, 0);
            var filter = new FilterBuilder(LogicalOperator.And)
                .On("createdon", date);

            var expr = filter.ToExpression();
            var condition = Assert.Single(expr.Conditions);
            var value = Assert.Single(condition.Values);
            Assert.Equal(date, value);
            Assert.Equal(date.TimeOfDay, ((DateTime)value).TimeOfDay);
        }

        [Fact]
        public void NullFilterBuilder_ThrowsException()
        {
            // Test null reference handling
            FilterBuilder nullBuilder = null;
            Assert.Throws<NullReferenceException>(() => nullBuilder.Equal("name", "Test"));
        }
    }
}