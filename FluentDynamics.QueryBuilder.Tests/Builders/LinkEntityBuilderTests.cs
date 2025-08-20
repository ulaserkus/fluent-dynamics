using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests.Builders
{

    public class LinkEntityBuilderTests
    {
        #region Column Selection

        [Fact]
        public void Select_AddsSpecifiedColumns()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.Select("firstname", "lastname"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.False(link.Columns.AllColumns);
            Assert.Contains("firstname", link.Columns.Columns);
            Assert.Contains("lastname", link.Columns.Columns);
        }

        [Fact]
        public void Select_WithNoAttributes_AddsNothing_AndDoesNotThrow()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.Select())
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.False(link.Columns.AllColumns);
            Assert.Empty(link.Columns.Columns);
        }

        [Fact]
        public void SelectAll_SetsAllColumnsTrue_AndClearsExplicitList()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.SelectAll())
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.True(link.Columns.AllColumns);
            Assert.Empty(link.Columns.Columns);
        }

        [Fact]
        public void SelectAllThenSelect_OverridesAllColumnsMode()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.SelectAll()
                          .Select("firstname"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.False(link.Columns.AllColumns);
            Assert.Single(link.Columns.Columns);
            Assert.Contains("firstname", link.Columns.Columns);
        }

        #endregion

        #region Alias

        [Fact]
        public void As_SetsAlias()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.As("pc"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.Equal("pc", link.EntityAlias);
        }

        [Fact]
        public void As_CalledTwice_UsesLastAlias()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.As("pc1").As("pc2"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.Equal("pc2", link.EntityAlias);
        }

        #endregion

        #region ForceSeek

        [Fact]
        public void ForceSeek_SetsForceSeekProperty()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.ForceSeek("ndx_contact_name"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.Equal("ndx_contact_name", link.ForceSeek);
        }

        [Fact]
        public void ForceSeek_Override_UsesLastValue()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.ForceSeek("ix1").ForceSeek("ix2"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.Equal("ix2", link.ForceSeek);
        }

        #endregion

        #region Ordering

        [Fact]
        public void OrderBy_AddsOrderExpression()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner,
                    l => l.OrderBy("firstname"))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            var order = Assert.Single(link.Orders);
            Assert.Equal("firstname", order.AttributeName);
            Assert.Equal(OrderType.Descending, order.OrderType);
        }

        [Fact]
        public void MultipleOperations_ChainedSuccessfully()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .As("pc")
                    .Select("firstname")
                    .OrderBy("firstname")
                    .ForceSeek("ndx_contact_firstname")
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .And(and => and.Condition("firstname", ConditionOperator.NotNull))
                    ))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            Assert.Equal("pc", link.EntityAlias);
            Assert.Contains("firstname", link.Columns.Columns);
            Assert.Single(link.Orders);
            Assert.Equal("ndx_contact_firstname", link.ForceSeek);

            Assert.Equal(LogicalOperator.And, link.LinkCriteria.FilterOperator);
            Assert.Single(link.LinkCriteria.Conditions); // statecode
            var nestedAnd = Assert.Single(link.LinkCriteria.Filters);
            Assert.Single(nestedAnd.Conditions); // firstname NotNull
        }

        #endregion

        #region Filtering

        [Fact]
        public void Where_SetsLinkCriteriaWithNestedFilters()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .Or(or => or
                            .Condition("firstname", ConditionOperator.Like, "A%")
                            .Condition("lastname", ConditionOperator.Like, "B%")
                        )))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            var criteria = link.LinkCriteria;
            Assert.NotNull(criteria);
            Assert.Equal(LogicalOperator.And, criteria.FilterOperator);
            Assert.Single(criteria.Conditions); // statecode
            var nested = Assert.Single(criteria.Filters);
            Assert.Equal(LogicalOperator.Or, nested.FilterOperator);
            Assert.Equal(2, nested.Conditions.Count);
        }

        [Fact]
        public void Where_CalledTwice_OverridesPreviousCriteria()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f.Condition("firstname", ConditionOperator.Equal, "Alice"))
                    .Where(f => f.Condition("lastname", ConditionOperator.Equal, "Smith")))
                .ToQueryExpression();

            var link = Assert.Single(q.LinkEntities);
            var criteria = link.LinkCriteria;
            var c = Assert.Single(criteria.Conditions);
            Assert.Equal("lastname", c.AttributeName);
            Assert.Equal(ConditionOperator.Equal, c.Operator);
            Assert.Single(c.Values);
            Assert.Equal("Smith", c.Values[0]);
        }

        #endregion

        #region Nesting

        [Fact]
        public void NestedLink_AddsChildLinkEntity()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e
                        .Select("subject").As("em")))
                .ToQueryExpression();

            var contactLink = Assert.Single(q.LinkEntities);
            var emailLink = Assert.Single(contactLink.LinkEntities);
            Assert.Equal("email", emailLink.LinkToEntityName);
            Assert.Contains("subject", emailLink.Columns.Columns);
            Assert.Equal("em", emailLink.EntityAlias);
        }

        [Fact]
        public void DeepNestedLinks_AllPersist()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e
                        .Link("activityparty", "activityid", "activityid", JoinOperator.Inner, ap => ap
                            .Select("partyid"))))
                .ToQueryExpression();

            var contact = Assert.Single(q.LinkEntities);
            var email = Assert.Single(contact.LinkEntities);
            var activityParty = Assert.Single(email.LinkEntities);

            Assert.Equal("activityparty", activityParty.LinkToEntityName);
            Assert.Contains("partyid", activityParty.Columns.Columns);
        }

        [Fact]
        public void SiblingNestedLinks_AllPresent()
        {
            var q = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e.Select("subject"))
                    .Link("task", "contactid", "regardingobjectid", JoinOperator.LeftOuter, t => t.Select("subject")))
                .ToQueryExpression();

            var contact = Assert.Single(q.LinkEntities);
            Assert.Equal(2, contact.LinkEntities.Count);
            Assert.Contains(contact.LinkEntities, le => le.LinkToEntityName == "email");
            Assert.Contains(contact.LinkEntities, le => le.LinkToEntityName == "task");
        }

        #endregion
    }
}