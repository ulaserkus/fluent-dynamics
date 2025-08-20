using FluentDynamics.QueryBuilder;
using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;

namespace FluentDynamics.Samples.NetFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string clientId = "<client-id>";
            string clientSecret = "<client-secret>";
            Uri serviceUri = new Uri("<org-url>");
            string connectionString = $"AuthType=ClientSecret;Url={serviceUri};ClientId={clientId};ClientSecret={clientSecret};";
            IOrganizationService service = new CrmServiceClient(connectionString);


            // Basic query to retrieve accounts with specific fields and conditions
            var basicQuery = Query.For("account")
                 .Select("name", "accountnumber", "telephone1")
                 .Where(f => f
                     .Condition("statecode", ConditionOperator.Equal, 0)
                 )
                 .OrderBy("name")
                 .Top(10);

            // Using FilterExpression for the same query
            var basicQueryWithFilterExpression = Query.For("account")
                 .Select("name", "accountnumber", "telephone1")
                 .Where(f => f
                     .Equal("statecode", 0)
                 )
                 .OrderBy("name")
                 .Top(10);

            var accountResults = basicQuery.RetrieveMultiple(service);

            //Complex filtering with nested conditions
            Guid accountId = Guid.NewGuid(); // Replace with an actual account ID
            var complexQuery = Query.For("contact")
            .Select("firstname", "lastname", "emailaddress1")
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .And(fa => fa
                            .Condition("createdon", ConditionOperator.LastXDays, 30)
                            .Condition("emailaddress1", ConditionOperator.NotNull)
                        )
                        .Or(fo => fo
                            .Condition("parentcustomerid", ConditionOperator.Equal, accountId)
                            .Condition("address1_city", ConditionOperator.Equal, "Seattle")
                        )
                    )
                    .OrderBy("lastname")
                    .OrderBy("firstname");

            // Complex filtering with nested conditions using FilterExpression
            var complexQueryWithFilterExpression = Query.For("contact")
            .Select("firstname", "lastname", "emailaddress1")
                    .Where(f => f
                        .Condition("statecode", ConditionOperator.Equal, 0)
                        .And(fa => fa
                            .LastXDays("createdon", 30)
                            .IsNotNull("emailaddress1")
                        )
                        .Or(fo => fo
                            .Equal("parentcustomerid", accountId)
                            .Equal("address1_city", "Seattle")
                        )
                    )
                    .OrderBy("lastname")
                    .OrderBy("firstname");

            // Joining multiple entities with specific conditions and aliases
            var joiningQueryquery = Query.For("opportunity")
                .Select("name", "estimatedvalue", "closeprobability")
                .Where(f => f
                    .Condition("statecode", ConditionOperator.Equal, 0)
                )
                .Link("account", "customerid", "accountid", JoinOperator.Inner, link =>
                {
                    link.Select("name", "accountnumber")
                        .As("account")
                        .Where(f => f
                            .Condition("statecode", ConditionOperator.Equal, 0)
                        );
                })
                .Link("contact", "customerid", "contactid", JoinOperator.LeftOuter, link =>
                {
                    link.Select("fullname", "emailaddress1")
                        .As("contact");
                });

            var opportunityResults = joiningQueryquery.RetrieveMultiple(service);


            // Get a specific page
            var page2 = basicQuery.RetrieveMultiple(service, pageNumber: 2, pageSize: 50);

            // Retrieve all pages automatically
            var allResults = basicQuery.RetrieveMultipleAllPages(service);

            // Using sync version
            var results = basicQuery.RetrieveMultiple(service);


            // Convert to list
            var entities = results.ToList();

            // Filter results
            var filteredEntities = results.Where(e => e.Contains("emailaddress1"));

            // Project to new form
            var names = results.Select(e => e.GetAttributeValue<string>("name"));

            // Get first matching entity
            var matchingContact = results.FirstOrDefault(e =>
                e.GetAttributeValue<string>("emailaddress1").Contains("example.com"));

            Console.WriteLine("EOP");
            // Keep the console window open
            Console.ReadLine();
        }
    }
}
