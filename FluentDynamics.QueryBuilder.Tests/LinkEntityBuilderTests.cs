using Microsoft.Xrm.Sdk.Query;
using Xunit;

namespace FluentDynamics.QueryBuilder.Tests
{
    public class LinkEntityBuilderTests
    {
        [Fact]
        public void Select_AddsSpecifiedColumns()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Select("firstname", "lastname"));

            var qx = query.ToQueryExpression();
            var link = Assert.Single(qx.LinkEntities);

            Assert.False(link.Columns.AllColumns);
            Assert.Contains("firstname", link.Columns.Columns);
            Assert.Contains("lastname", link.Columns.Columns);
        }

        [Fact]
        public void SelectAll_SetsAllColumnsTrue()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .SelectAll());

            var link = query.ToQueryExpression().LinkEntities.Single();
            Assert.True(link.Columns.AllColumns);
            Assert.Empty(link.Columns.Columns); 
        }

        [Fact]
        public void As_SetsAlias()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .As("pc"));

            var link = query.ToQueryExpression().LinkEntities.Single();
            Assert.Equal("pc", link.EntityAlias);
        }

        [Fact]
        public void ForceSeek_SetsForceSeekProperty()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .ForceSeek("ndx_contact_name"));

            var link = query.ToQueryExpression().LinkEntities.Single();
            Assert.Equal("ndx_contact_name", link.ForceSeek);
        }

        [Fact]
        public void OrderBy_AddsOrderExpression()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .OrderBy("firstname", OrderType.Descending));

            var link = query.ToQueryExpression().LinkEntities.Single();
            var order = Assert.Single(link.Orders);
            Assert.Equal("firstname", order.AttributeName);
            Assert.Equal(OrderType.Descending, order.OrderType);
        }

        [Fact]
        public void Where_SetsLinkCriteriaWithNestedFilters()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .Or(or => or
                            .Condition("firstname", ConditionOperator.Like, "A%")
                            .Condition("lastname", ConditionOperator.Like, "B%")
                        )
                    ));

            var link = query.ToQueryExpression().LinkEntities.Single();
            var criteria = link.LinkCriteria;

            Assert.NotNull(criteria);
            Assert.Equal(LogicalOperator.And, criteria.FilterOperator);
            Assert.Single(criteria.Conditions); // statecode
            var nested = Assert.Single(criteria.Filters);
            Assert.Equal(LogicalOperator.Or, nested.FilterOperator);
            Assert.Equal(2, nested.Conditions.Count); // firstname / lastname
        }

        [Fact]
        public void NestedLink_AddsChildLinkEntity()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e
                        .Select("subject")
                        .As("em")
                    ));

            var parent = query.ToQueryExpression().LinkEntities.Single();
            var child = Assert.Single(parent.LinkEntities);
            Assert.Equal("email", child.LinkToEntityName);
            Assert.Contains("subject", child.Columns.Columns);
            Assert.Equal("em", child.EntityAlias);
        }

        [Fact]
        public void MultipleOperations_ChainedSuccessfully()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .As("pc")
                    .Select("firstname")
                    .OrderBy("firstname")
                    .ForceSeek("ndx_contact_firstname")
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .And(and => and.Condition("firstname", ConditionOperator.NotNull))
                    )
                );

            var link = query.ToQueryExpression().LinkEntities.Single();

            Assert.Equal("pc", link.EntityAlias);
            Assert.Contains("firstname", link.Columns.Columns);
            Assert.Single(link.Orders);
            Assert.Equal("ndx_contact_firstname", link.ForceSeek);

            // Criteria
            Assert.Equal(LogicalOperator.And, link.LinkCriteria.FilterOperator);
            Assert.Equal(1, link.LinkCriteria.Conditions.Count); // statecode
            var nestedAnd = Assert.Single(link.LinkCriteria.Filters);
            Assert.Equal(1, nestedAnd.Conditions.Count); // firstname NotNull
        }

        [Fact]
        public void Where_CalledTwice_OverridesPreviousCriteria()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Where(f => f.Condition("firstname", ConditionOperator.Equal, "Alice"))
                    .Where(f => f.Condition("lastname", ConditionOperator.Equal, "Smith"))
                );

            var link = query.ToQueryExpression().LinkEntities.Single();

            // Beklenen: ikinci Where ilk kriteri ezmiş olsun
            Assert.Single(link.LinkCriteria.Conditions);
            var finalCond = link.LinkCriteria.Conditions.First();
            Assert.Equal("lastname", finalCond.AttributeName);
            Assert.Equal(ConditionOperator.Equal, finalCond.Operator);
            Assert.Single(finalCond.Values);
            Assert.Equal("Smith", finalCond.Values[0]);
        }

        [Fact]
        public void Select_WithNoAttributes_DoesNotThrowAndAddsNothing()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l.Select());

            var link = query.ToQueryExpression().LinkEntities.Single();
            Assert.False(link.Columns.AllColumns);
            Assert.Empty(link.Columns.Columns);
        }

        [Fact]
        public void DeepNestedLinks_AllPersist()
        {
            var query = Query.For("account")
                .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, l => l
                    .Link("email", "contactid", "regardingobjectid", JoinOperator.LeftOuter, e => e
                        .Link("activityparty", "activityid", "activityid", JoinOperator.Inner, ap => ap
                            .Select("partyid")
                        )
                    )
                );

            var contactLink = query.ToQueryExpression().LinkEntities.Single();
            var emailLink = Assert.Single(contactLink.LinkEntities);
            var activityPartyLink = Assert.Single(emailLink.LinkEntities);

            Assert.Equal("activityparty", activityPartyLink.LinkToEntityName);
            Assert.Contains("partyid", activityPartyLink.Columns.Columns);
        }
    }
}