using FluentDynamics.QueryBuilder.Builders;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace FluentDynamics.QueryBuilder.Extensions
{
    /// <summary>
    /// Extension methods that provide intuitive operator syntax for FilterBuilder
    /// </summary>
    public static class FilterBuilderExtensions
    {
        #region Basic Comparison Operators

        /// <summary>
        /// Adds an equality condition (==)
        /// </summary>
        public static FilterBuilder Equal(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.Equal, value);

        /// <summary>
        /// Adds an inequality condition (!=)
        /// </summary>
        public static FilterBuilder NotEqual(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.NotEqual, value);

        /// <summary>
        /// Adds a greater than condition (>)
        /// </summary>
        public static FilterBuilder GreaterThan(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.GreaterThan, value);

        /// <summary>
        /// Adds a less than condition (<)
        /// </summary>
        public static FilterBuilder LessThan(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.LessThan, value);

        /// <summary>
        /// Adds a greater than or equal condition (>=)
        /// </summary>
        public static FilterBuilder GreaterEqual(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.GreaterEqual, value);

        /// <summary>
        /// Adds a less than or equal condition (<=)
        /// </summary>
        public static FilterBuilder LessEqual(this FilterBuilder builder, string attribute, object value)
            => builder.Condition(attribute, ConditionOperator.LessEqual, value);

        #endregion

        #region Null Operators

        /// <summary>
        /// Adds a NULL condition
        /// </summary>
        public static FilterBuilder IsNull(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Null);

        /// <summary>
        /// Adds a NOT NULL condition
        /// </summary>
        public static FilterBuilder IsNotNull(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NotNull);

        #endregion

        #region String Comparison Operators

        /// <summary>
        /// Adds a LIKE condition
        /// </summary>
        public static FilterBuilder Like(this FilterBuilder builder, string attribute, string pattern)
            => builder.Condition(attribute, ConditionOperator.Like, pattern);

        /// <summary>
        /// Adds a NOT LIKE condition
        /// </summary>
        public static FilterBuilder NotLike(this FilterBuilder builder, string attribute, string pattern)
            => builder.Condition(attribute, ConditionOperator.NotLike, pattern);

        /// <summary>
        /// Adds a BeginsWith condition
        /// </summary>
        public static FilterBuilder BeginsWith(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.BeginsWith, value);

        /// <summary>
        /// Adds a DoesNotBeginWith condition
        /// </summary>
        public static FilterBuilder DoesNotBeginWith(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.DoesNotBeginWith, value);

        /// <summary>
        /// Adds an EndsWith condition
        /// </summary>
        public static FilterBuilder EndsWith(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.EndsWith, value);

        /// <summary>
        /// Adds a DoesNotEndWith condition
        /// </summary>
        public static FilterBuilder DoesNotEndWith(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.DoesNotEndWith, value);

        /// <summary>
        /// Adds a Contains condition (for full-text indexed columns)
        /// </summary>
        public static FilterBuilder Contains(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.Contains, value);

        /// <summary>
        /// Adds a DoesNotContain condition
        /// </summary>
        public static FilterBuilder DoesNotContain(this FilterBuilder builder, string attribute, string value)
            => builder.Condition(attribute, ConditionOperator.DoesNotContain, value);

        #endregion

        #region Collection Operators

        /// <summary>
        /// Adds an IN condition
        /// </summary>
        public static FilterBuilder In(this FilterBuilder builder, string attribute, params object[] values)
            => builder.Condition(attribute, ConditionOperator.In, values);

        /// <summary>
        /// Adds a NOT IN condition
        /// </summary>
        public static FilterBuilder NotIn(this FilterBuilder builder, string attribute, params object[] values)
            => builder.Condition(attribute, ConditionOperator.NotIn, values);

        /// <summary>
        /// Adds a BETWEEN condition
        /// </summary>
        public static FilterBuilder Between(this FilterBuilder builder, string attribute, object lowerBound, object upperBound)
            => builder.Condition(attribute, ConditionOperator.Between, new object[] { lowerBound, upperBound });

        /// <summary>
        /// Adds a NOT BETWEEN condition
        /// </summary>
        public static FilterBuilder NotBetween(this FilterBuilder builder, string attribute, object lowerBound, object upperBound)
            => builder.Condition(attribute, ConditionOperator.NotBetween, new object[] { lowerBound, upperBound });

        /// <summary>
        /// Adds a ContainValues condition for choice/multiselect attributes
        /// </summary>
        public static FilterBuilder ContainValues(this FilterBuilder builder, string attribute, params int[] values)
            => builder.Condition(attribute, ConditionOperator.ContainValues, values);

        /// <summary>
        /// Adds a DoesNotContainValues condition for choice/multiselect attributes
        /// </summary>
        public static FilterBuilder DoesNotContainValues(this FilterBuilder builder, string attribute, params int[] values)
            => builder.Condition(attribute, ConditionOperator.DoesNotContainValues, values);

        #endregion

        #region Date Operators - Absolute

        /// <summary>
        /// Adds an On condition for date
        /// </summary>
        public static FilterBuilder On(this FilterBuilder builder, string attribute, DateTime date)
            => builder.Condition(attribute, ConditionOperator.On, date);

        /// <summary>
        /// Adds a NotOn condition for date
        /// </summary>
        public static FilterBuilder NotOn(this FilterBuilder builder, string attribute, DateTime date)
            => builder.Condition(attribute, ConditionOperator.NotOn, date);

        /// <summary>
        /// Adds an OnOrBefore condition for date
        /// </summary>
        public static FilterBuilder OnOrBefore(this FilterBuilder builder, string attribute, DateTime date)
            => builder.Condition(attribute, ConditionOperator.OnOrBefore, date);

        /// <summary>
        /// Adds an OnOrAfter condition for date
        /// </summary>
        public static FilterBuilder OnOrAfter(this FilterBuilder builder, string attribute, DateTime date)
            => builder.Condition(attribute, ConditionOperator.OnOrAfter, date);

        #endregion

        #region Date Operators - Relative (No Parameters)

        /// <summary>
        /// Adds a Yesterday condition
        /// </summary>
        public static FilterBuilder Yesterday(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Yesterday);

        /// <summary>
        /// Adds a Today condition
        /// </summary>
        public static FilterBuilder Today(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Today);

        /// <summary>
        /// Adds a Tomorrow condition
        /// </summary>
        public static FilterBuilder Tomorrow(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Tomorrow);

        /// <summary>
        /// Adds a Last7Days condition
        /// </summary>
        public static FilterBuilder Last7Days(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Last7Days);

        /// <summary>
        /// Adds a Next7Days condition
        /// </summary>
        public static FilterBuilder Next7Days(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.Next7Days);

        /// <summary>
        /// Adds a LastWeek condition
        /// </summary>
        public static FilterBuilder LastWeek(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.LastWeek);

        /// <summary>
        /// Adds a ThisWeek condition
        /// </summary>
        public static FilterBuilder ThisWeek(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.ThisWeek);

        /// <summary>
        /// Adds a NextWeek condition
        /// </summary>
        public static FilterBuilder NextWeek(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NextWeek);

        /// <summary>
        /// Adds a LastMonth condition
        /// </summary>
        public static FilterBuilder LastMonth(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.LastMonth);

        /// <summary>
        /// Adds a ThisMonth condition
        /// </summary>
        public static FilterBuilder ThisMonth(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.ThisMonth);

        /// <summary>
        /// Adds a NextMonth condition
        /// </summary>
        public static FilterBuilder NextMonth(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NextMonth);

        /// <summary>
        /// Adds a LastYear condition
        /// </summary>
        public static FilterBuilder LastYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.LastYear);

        /// <summary>
        /// Adds a ThisYear condition
        /// </summary>
        public static FilterBuilder ThisYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.ThisYear);

        /// <summary>
        /// Adds a NextYear condition
        /// </summary>
        public static FilterBuilder NextYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NextYear);

        #endregion

        #region Date Operators - Relative (With Parameters)

        /// <summary>
        /// Adds a LastXHours condition
        /// </summary>
        public static FilterBuilder LastXHours(this FilterBuilder builder, string attribute, int hours)
            => builder.Condition(attribute, ConditionOperator.LastXHours, hours);

        /// <summary>
        /// Adds a NextXHours condition
        /// </summary>
        public static FilterBuilder NextXHours(this FilterBuilder builder, string attribute, int hours)
            => builder.Condition(attribute, ConditionOperator.NextXHours, hours);

        /// <summary>
        /// Adds a LastXDays condition
        /// </summary>
        public static FilterBuilder LastXDays(this FilterBuilder builder, string attribute, int days)
            => builder.Condition(attribute, ConditionOperator.LastXDays, days);

        /// <summary>
        /// Adds a NextXDays condition
        /// </summary>
        public static FilterBuilder NextXDays(this FilterBuilder builder, string attribute, int days)
            => builder.Condition(attribute, ConditionOperator.NextXDays, days);

        /// <summary>
        /// Adds a LastXWeeks condition
        /// </summary>
        public static FilterBuilder LastXWeeks(this FilterBuilder builder, string attribute, int weeks)
            => builder.Condition(attribute, ConditionOperator.LastXWeeks, weeks);

        /// <summary>
        /// Adds a NextXWeeks condition
        /// </summary>
        public static FilterBuilder NextXWeeks(this FilterBuilder builder, string attribute, int weeks)
            => builder.Condition(attribute, ConditionOperator.NextXWeeks, weeks);

        /// <summary>
        /// Adds a LastXMonths condition
        /// </summary>
        public static FilterBuilder LastXMonths(this FilterBuilder builder, string attribute, int months)
            => builder.Condition(attribute, ConditionOperator.LastXMonths, months);

        /// <summary>
        /// Adds a NextXMonths condition
        /// </summary>
        public static FilterBuilder NextXMonths(this FilterBuilder builder, string attribute, int months)
            => builder.Condition(attribute, ConditionOperator.NextXMonths, months);

        /// <summary>
        /// Adds a LastXYears condition
        /// </summary>
        public static FilterBuilder LastXYears(this FilterBuilder builder, string attribute, int years)
            => builder.Condition(attribute, ConditionOperator.LastXYears, years);

        /// <summary>
        /// Adds a NextXYears condition
        /// </summary>
        public static FilterBuilder NextXYears(this FilterBuilder builder, string attribute, int years)
            => builder.Condition(attribute, ConditionOperator.NextXYears, years);

        /// <summary>
        /// Adds an OlderThanXMinutes condition
        /// </summary>
        public static FilterBuilder OlderThanXMinutes(this FilterBuilder builder, string attribute, int minutes)
            => builder.Condition(attribute, ConditionOperator.OlderThanXMinutes, minutes);

        /// <summary>
        /// Adds an OlderThanXHours condition
        /// </summary>
        public static FilterBuilder OlderThanXHours(this FilterBuilder builder, string attribute, int hours)
            => builder.Condition(attribute, ConditionOperator.OlderThanXHours, hours);

        /// <summary>
        /// Adds an OlderThanXDays condition
        /// </summary>
        public static FilterBuilder OlderThanXDays(this FilterBuilder builder, string attribute, int days)
            => builder.Condition(attribute, ConditionOperator.OlderThanXDays, days);

        /// <summary>
        /// Adds an OlderThanXWeeks condition
        /// </summary>
        public static FilterBuilder OlderThanXWeeks(this FilterBuilder builder, string attribute, int weeks)
            => builder.Condition(attribute, ConditionOperator.OlderThanXWeeks, weeks);

        /// <summary>
        /// Adds an OlderThanXMonths condition
        /// </summary>
        public static FilterBuilder OlderThanXMonths(this FilterBuilder builder, string attribute, int months)
            => builder.Condition(attribute, ConditionOperator.OlderThanXMonths, months);

        /// <summary>
        /// Adds an OlderThanXYears condition
        /// </summary>
        public static FilterBuilder OlderThanXYears(this FilterBuilder builder, string attribute, int years)
            => builder.Condition(attribute, ConditionOperator.OlderThanXYears, years);

        #endregion

        #region Fiscal Period Operators - No Parameters

        /// <summary>
        /// Adds a ThisFiscalYear condition
        /// </summary>
        public static FilterBuilder ThisFiscalYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.ThisFiscalYear);

        /// <summary>
        /// Adds a ThisFiscalPeriod condition
        /// </summary>
        public static FilterBuilder ThisFiscalPeriod(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.ThisFiscalPeriod);

        /// <summary>
        /// Adds a NextFiscalYear condition
        /// </summary>
        public static FilterBuilder NextFiscalYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NextFiscalYear);

        /// <summary>
        /// Adds a NextFiscalPeriod condition
        /// </summary>
        public static FilterBuilder NextFiscalPeriod(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NextFiscalPeriod);

        /// <summary>
        /// Adds a LastFiscalYear condition
        /// </summary>
        public static FilterBuilder LastFiscalYear(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.LastFiscalYear);

        /// <summary>
        /// Adds a LastFiscalPeriod condition
        /// </summary>
        public static FilterBuilder LastFiscalPeriod(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.LastFiscalPeriod);

        #endregion

        #region Fiscal Period Operators - With Parameters

        /// <summary>
        /// Adds a LastXFiscalYears condition
        /// </summary>
        public static FilterBuilder LastXFiscalYears(this FilterBuilder builder, string attribute, int years)
            => builder.Condition(attribute, ConditionOperator.LastXFiscalYears, years);

        /// <summary>
        /// Adds a LastXFiscalPeriods condition
        /// </summary>
        public static FilterBuilder LastXFiscalPeriods(this FilterBuilder builder, string attribute, int periods)
            => builder.Condition(attribute, ConditionOperator.LastXFiscalPeriods, periods);

        /// <summary>
        /// Adds a NextXFiscalYears condition
        /// </summary>
        public static FilterBuilder NextXFiscalYears(this FilterBuilder builder, string attribute, int years)
            => builder.Condition(attribute, ConditionOperator.NextXFiscalYears, years);

        /// <summary>
        /// Adds a NextXFiscalPeriods condition
        /// </summary>
        public static FilterBuilder NextXFiscalPeriods(this FilterBuilder builder, string attribute, int periods)
            => builder.Condition(attribute, ConditionOperator.NextXFiscalPeriods, periods);

        /// <summary>
        /// Adds an InFiscalYear condition
        /// </summary>
        public static FilterBuilder InFiscalYear(this FilterBuilder builder, string attribute, int year)
            => builder.Condition(attribute, ConditionOperator.InFiscalYear, year);

        /// <summary>
        /// Adds an InFiscalPeriod condition
        /// </summary>
        public static FilterBuilder InFiscalPeriod(this FilterBuilder builder, string attribute, int period)
            => builder.Condition(attribute, ConditionOperator.InFiscalPeriod, period);

        /// <summary>
        /// Adds an InFiscalPeriodAndYear condition
        /// </summary>
        public static FilterBuilder InFiscalPeriodAndYear(this FilterBuilder builder, string attribute, int period, int year)
            => builder.Condition(attribute, ConditionOperator.InFiscalPeriodAndYear, new object[] { period, year });

        /// <summary>
        /// Adds an InOrBeforeFiscalPeriodAndYear condition
        /// </summary>
        public static FilterBuilder InOrBeforeFiscalPeriodAndYear(this FilterBuilder builder, string attribute, int period, int year)
            => builder.Condition(attribute, ConditionOperator.InOrBeforeFiscalPeriodAndYear, new object[] { period, year });

        /// <summary>
        /// Adds an InOrAfterFiscalPeriodAndYear condition
        /// </summary>
        public static FilterBuilder InOrAfterFiscalPeriodAndYear(this FilterBuilder builder, string attribute, int period, int year)
            => builder.Condition(attribute, ConditionOperator.InOrAfterFiscalPeriodAndYear, new object[] { period, year });

        #endregion

        #region User and Business Operators

        /// <summary>
        /// Adds an EqualUserId condition
        /// </summary>
        public static FilterBuilder EqualUserId(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserId);

        /// <summary>
        /// Adds a NotEqualUserId condition
        /// </summary>
        public static FilterBuilder NotEqualUserId(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NotEqualUserId);

        /// <summary>
        /// Adds an EqualUserTeams condition
        /// </summary>
        public static FilterBuilder EqualUserTeams(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserTeams);

        /// <summary>
        /// Adds an EqualUserOrUserTeams condition
        /// </summary>
        public static FilterBuilder EqualUserOrUserTeams(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserOrUserTeams);

        /// <summary>
        /// Adds an EqualUserOrUserHierarchy condition
        /// </summary>
        public static FilterBuilder EqualUserOrUserHierarchy(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserOrUserHierarchy);

        /// <summary>
        /// Adds an EqualUserOrUserHierarchyAndTeams condition
        /// </summary>
        public static FilterBuilder EqualUserOrUserHierarchyAndTeams(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserOrUserHierarchyAndTeams);

        /// <summary>
        /// Adds an EqualBusinessId condition
        /// </summary>
        public static FilterBuilder EqualBusinessId(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualBusinessId);

        /// <summary>
        /// Adds a NotEqualBusinessId condition
        /// </summary>
        public static FilterBuilder NotEqualBusinessId(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.NotEqualBusinessId);

        /// <summary>
        /// Adds an EqualUserLanguage condition
        /// </summary>
        public static FilterBuilder EqualUserLanguage(this FilterBuilder builder, string attribute)
            => builder.Condition(attribute, ConditionOperator.EqualUserLanguage);

        #endregion

        #region Hierarchy Operators

        /// <summary>
        /// Adds an Under condition
        /// </summary>
        public static FilterBuilder Under(this FilterBuilder builder, string attribute, Guid recordId)
            => builder.Condition(attribute, ConditionOperator.Under, recordId);

        /// <summary>
        /// Adds a NotUnder condition
        /// </summary>
        public static FilterBuilder NotUnder(this FilterBuilder builder, string attribute, Guid recordId)
            => builder.Condition(attribute, ConditionOperator.NotUnder, recordId);

        /// <summary>
        /// Adds an UnderOrEqual condition
        /// </summary>
        public static FilterBuilder UnderOrEqual(this FilterBuilder builder, string attribute, Guid recordId)
            => builder.Condition(attribute, ConditionOperator.UnderOrEqual, recordId);

        /// <summary>
        /// Adds an Above condition
        /// </summary>
        public static FilterBuilder Above(this FilterBuilder builder, string attribute, Guid recordId)
            => builder.Condition(attribute, ConditionOperator.Above, recordId);

        /// <summary>
        /// Adds an AboveOrEqual condition
        /// </summary>
        public static FilterBuilder AboveOrEqual(this FilterBuilder builder, string attribute, Guid recordId)
            => builder.Condition(attribute, ConditionOperator.AboveOrEqual, recordId);

        #endregion

        #region Bit Mask Operators

        /// <summary>
        /// Adds a Mask condition
        /// </summary>
        public static FilterBuilder Mask(this FilterBuilder builder, string attribute, int mask)
            => builder.Condition(attribute, ConditionOperator.Mask, mask);

        /// <summary>
        /// Adds a NotMask condition
        /// </summary>
        public static FilterBuilder NotMask(this FilterBuilder builder, string attribute, int mask)
            => builder.Condition(attribute, ConditionOperator.NotMask, mask);

        #endregion

    }
}