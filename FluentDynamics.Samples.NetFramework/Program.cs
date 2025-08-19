using FluentDynamics.QueryBuilder;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Threading.Tasks;

namespace FluentDynamics.Samples.NetFramework
{
    internal class Program
    {
        async static Task Main(string[] args)
        {
            string clientId = "<client-id>";
            string clientSecret = "<client-secret>";
            Uri serviceUri = new Uri("<org-url>");
            string connectionString = $"AuthType=ClientSecret;Url={serviceUri};ClientId={clientId};ClientSecret={clientSecret};";
            IOrganizationService service = new CrmServiceClient(connectionString);


            // Basic query to retrieve accounts with specific fields and conditions
            var basicQuery = Query.For("account")
                .Select("name", "accountnumber", "telephone1")
                .Where("statecode", ConditionOperator.Equal, 0)
                .OrderBy("name")
                .Top(10);

            var accountResults = await basicQuery.RetrieveMultiple(service).ToListAsync();

            //Complex filtering with nested conditions
            Guid accountId = Guid.NewGuid(); // Replace with an actual account ID
            var complexQuery = Query.For("contact")
                .Select("firstname", "lastname", "emailaddress1")
                .Where("statecode", ConditionOperator.Equal, 0)
                .And(q =>
                {
                    q.Where("createdon", ConditionOperator.LastXDays, 30);
                    q.Where("emailaddress1", ConditionOperator.NotNull);
                })
                .Or(q =>
                {
                    q.Where("parentcustomerid", ConditionOperator.Equal, accountId);
                    q.Where("address1_city", ConditionOperator.Equal, "Seattle");
                })
                .OrderBy("lastname")
                .OrderBy("firstname");

            var contactResults = await complexQuery.RetrieveMultiple(service).ToListAsync();

            // Joining multiple entities with specific conditions and aliases
            var joiningQueryquery = Query.For("opportunity")
                .Select("name", "estimatedvalue", "closeprobability")
                .Where("statecode", ConditionOperator.Equal, 0)
                .Link("account", "customerid", "accountid", JoinOperator.Inner, link =>
                {
                    link.Select("name", "accountnumber")
                        .As("account")
                        .Where("statecode", ConditionOperator.Equal, 0);
                })
                .Link("contact", "customerid", "contactid", JoinOperator.LeftOuter, link =>
                {
                    link.Select("fullname", "emailaddress1")
                        .As("contact");
                });

            var opportunityResults = await joiningQueryquery.RetrieveMultiple(service).ToListAsync();


            // Get a specific page
            var page2 = basicQuery.RetrieveMultiple(service, pageNumber: 2, pageSize: 50);

            // Retrieve all pages automatically
            var allResults = basicQuery.RetrieveMultipleAllPages(service);

            // Using async version
            var results = await basicQuery.RetrieveMultipleAsync(service);


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
