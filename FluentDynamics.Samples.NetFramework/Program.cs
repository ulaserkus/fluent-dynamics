using FluentDynamics.QueryBuilder;
using FluentDynamics.QueryBuilder.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;

namespace FluentDynamics.Samples.NetFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FluentDynamics.QueryBuilder Examples\n");

            try
            {
                // Create Dataverse connection
                IOrganizationService service = ConnectToDataverse();

                // Run different query examples
                RunBasicQueryExample(service);
                RunFilterBuilderExample(service);
                RunComplexFilterExample(service);
                RunJoinExample(service);
                RunPaginationExample(service);
                RunLinqStyleOperationsExample(service);

                // Advanced examples
                RunAdvancedFilterExample(service);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }

            Console.WriteLine("\nExamples completed. Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Example of connecting to Dataverse
        /// </summary>
        private static IOrganizationService ConnectToDataverse()
        {
            Console.WriteLine("Connecting to Dataverse...");

            // Set your own parameters
            string clientId = "<client-id>";
            string clientSecret = "<client-secret>";
            Uri serviceUri = new Uri("<org-url>");
            string connectionString = $"AuthType=ClientSecret;Url={serviceUri};ClientId={clientId};ClientSecret={clientSecret};";

            // Create connection using connection string
            IOrganizationService service = new CrmServiceClient(connectionString);

            if ((service as CrmServiceClient).IsReady)
            {
                Console.WriteLine("Connection successful\n");
            }
            else
            {
                throw new Exception("Could not connect to Dataverse. Check your connection parameters.");
            }

            return service;
        }

        /// <summary>
        /// Basic query example
        /// </summary>
        private static void RunBasicQueryExample(IOrganizationService service)
        {
            Console.WriteLine("1. Basic Query Example");
            Console.WriteLine("---------------------");

            // Basic query: Select active accounts
            var basicQuery = Query.For("account")
                .Select("name", "accountnumber", "telephone1")
                .Where(f => f.Equal("statecode", 0))  // Active records
                .OrderBy("name")
                .Top(5);  // Get only first 5 records

            Console.WriteLine("Query: Get first 5 active accounts");

            // Execute the query
            var accounts = basicQuery.RetrieveMultiple(service);

            // Display results
            Console.WriteLine($"Result: {accounts.Entities.Count} records found");
            foreach (var account in accounts.Entities)
            {
                Console.WriteLine($"  - {account.GetAttributeValue<string>("name")} " +
                                 $"(Account #: {account.GetAttributeValue<string>("accountnumber")})");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Example using FilterBuilder extensions
        /// </summary>
        private static void RunFilterBuilderExample(IOrganizationService service)
        {
            Console.WriteLine("2. FilterBuilder Extensions Example");
            Console.WriteLine("----------------------------------");

            // Create query with FilterBuilder extension methods
            var query = Query.For("account")
                .Select("name", "revenue", "createdon")
                .Where(f => f
                    .Equal("statecode", 0)                  // Active records
                    .GreaterThan("revenue", 10000)          // Revenue more than 10,000
                    .LastXMonths("createdon", 6)            // Created in last 6 months
                    .Or(o => o
                        .Like("name", "%Inc%")              // Name contains 'Inc'
                        .Contains("description", "partner") // Description contains 'partner'
                    )
                )
                .OrderBy("revenue")   // Order by revenue descending
                .Top(5);

            Console.WriteLine("Query: Accounts created in last 6 months with revenue > 10,000 " +
                            "OR accounts with 'Inc' in their name");

            // Execute the query
            var results = query.RetrieveMultiple(service);

            // Display results
            Console.WriteLine($"Result: {results.Entities.Count} records found");
            foreach (var account in results.Entities)
            {
                var revenue = account.GetAttributeValue<Money>("revenue")?.Value;
                Console.WriteLine($"  - {account.GetAttributeValue<string>("name")}" +
                                 $" (Revenue: {(revenue.HasValue ? revenue.Value.ToString("C") : "Not specified")})");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Complex filtering example (nested AND/OR conditions)
        /// </summary>
        private static void RunComplexFilterExample(IOrganizationService service)
        {
            Console.WriteLine("3. Complex Filtering Example");
            Console.WriteLine("---------------------------");

            // Example GUID (in a real application this would be an actual Account ID)
            Guid sampleAccountId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            // Create query with complex filtering
            var query = Query.For("contact")
                .Select("firstname", "lastname", "emailaddress1", "createdon")
                .Where(f => f
                    .Equal("statecode", 0)   // Active contacts
                    .And(fa => fa            // AND group
                        .LastXDays("createdon", 30)
                        .IsNotNull("emailaddress1")
                    )
                    .Or(fo => fo             // OR group
                        .Equal("parentcustomerid", sampleAccountId)
                        .Equal("address1_city", "Seattle")
                    )
                )
                .OrderBy("lastname")
                .OrderBy("firstname");

            Console.WriteLine("Query: Contacts created in last 30 days AND with email address " +
                            "OR contacts related to specific account OR contacts living in Seattle");

            try
            {
                // Execute the query
                var results = query.RetrieveMultiple(service);

                // Display results
                Console.WriteLine($"Result: {results.Entities.Count} records found");
                foreach (var contact in results.Entities)
                {
                    Console.WriteLine($"  - {contact.GetAttributeValue<string>("firstname")} " +
                                     $"{contact.GetAttributeValue<string>("lastname")} " +
                                     $"({contact.GetAttributeValue<string>("emailaddress1")})");
                }
            }
            catch (Exception ex)
            {
                // This example might fail due to invalid GUID
                Console.WriteLine($"  Note: This example could not be executed (invalid GUID): {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Join (LinkEntity) example
        /// </summary>
        private static void RunJoinExample(IOrganizationService service)
        {
            Console.WriteLine("4. Joining Related Tables Example");
            Console.WriteLine("--------------------------------");

            // Create query with relationships between multiple tables
            var query = Query.For("opportunity")
                .Select("name", "estimatedvalue", "closeprobability")
                .Where(f => f.Equal("statecode", 0))
                .InnerJoin("account", "customerid", "accountid", link =>
                {
                    // Inner join to account table
                    link.Select("name", "accountnumber")
                        .As("account")  // Returns column names with account_ prefix
                        .Where(f => f.Equal("statecode", 0));
                })
                .LeftOuterJoin("contact", "customerid", "contactid", link =>
                {
                    // Left outer join to contact table (optional)
                    link.Select("fullname", "emailaddress1")
                        .As("contact");  // Returns column names with contact_ prefix
                });

            Console.WriteLine("Query: Active opportunities with their related accounts and contacts");

            // Execute the query
            var results = query.RetrieveMultiple(service);

            // Display results
            Console.WriteLine($"Result: {results.Entities.Count} records found");
            foreach (var opportunity in results.Entities)
            {
                var value = opportunity.GetAttributeValue<Money>("estimatedvalue")?.Value;
                var accountName = opportunity.GetAttributeValue<AliasedValue>("account.name")?.Value as string;
                var contactName = opportunity.GetAttributeValue<AliasedValue>("contact.fullname")?.Value as string;

                Console.WriteLine($"  - Opportunity: {opportunity.GetAttributeValue<string>("name")} " +
                                 $"(Est. Value: {(value.HasValue ? value.Value.ToString("C") : "Not specified")})");
                Console.WriteLine($"    * Account: {accountName ?? "Not specified"}");
                Console.WriteLine($"    * Contact: {contactName ?? "Not specified"}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Pagination example
        /// </summary>
        private static void RunPaginationExample(IOrganizationService service)
        {
            Console.WriteLine("5. Pagination Example");
            Console.WriteLine("--------------------");

            // Simple query for pagination
            var query = Query.For("account")
                .Select("name")
                .OrderBy("name")
                .Top(1000); // Maximum 1000 records

            Console.WriteLine("Retrieving all accounts with pagination (page size: 2)");

            // Pagination parameters
            int pageNumber = 1;
            int pageSize = 2; // Small page size for demonstration
            string pagingCookie = null;
            bool moreRecords;
            int totalRecords = 0;

            Console.WriteLine("\nPages:");

            do
            {
                // Retrieve a specific page
                var pagedResults = query.RetrieveMultiple(service, pageNumber, pageSize);
                var pageRecords = pagedResults.Entities.Count;
                totalRecords += pageRecords;

                // Show records in the page
                Console.WriteLine($"  Page {pageNumber}: {pageRecords} records");
                foreach (var account in pagedResults.Entities)
                {
                    Console.WriteLine($"    - {account.GetAttributeValue<string>("name")}");
                }

                // Update for next page
                moreRecords = pagedResults.MoreRecords;
                pagingCookie = pagedResults.PagingCookie;
                pageNumber++;

                if (pageNumber > 3) // Show maximum 3 pages to keep example short
                {
                    Console.WriteLine("  ... (more not shown)");
                    break;
                }
            }
            while (moreRecords);

            Console.WriteLine($"\nTotal: {totalRecords} records shown" +
                            (moreRecords ? " (more records available)" : ""));

            Console.WriteLine();
        }

        /// <summary>
        /// LINQ-style operations example
        /// </summary>
        private static void RunLinqStyleOperationsExample(IOrganizationService service)
        {
            Console.WriteLine("6. LINQ-Style Operations Example");
            Console.WriteLine("-------------------------------");

            // Simple query
            var query = Query.For("contact")
                .Select("firstname", "lastname", "emailaddress1", "createdon")
                .Where(f => f.Equal("statecode", 0))
                .OrderByDesc("createdon")
                .Top(50);

            Console.WriteLine("LINQ-style operations on 50 most recently created contacts");

            // Execute the query
            var results = query.RetrieveMultiple(service);

            Console.WriteLine("\nExample 1: ToList() - Convert all results to a list");
            var contactList = results.ToList();
            Console.WriteLine($"  Result: {contactList.Count} records converted to list");

            Console.WriteLine("\nExample 2: Where() - Filter contacts with email address");
            var contactsWithEmail = results.Where(e => e.Contains("emailaddress1")).ToList();
            Console.WriteLine($"  Result: {contactsWithEmail.Count} contacts have email address");

            Console.WriteLine("\nExample 3: Select() - Project name and email addresses");
            var contactInfo = results.Select(e => new
            {
                Name = $"{e.GetAttributeValue<string>("firstname")} {e.GetAttributeValue<string>("lastname")}",
                Email = e.GetAttributeValue<string>("emailaddress1")
            }).Take(3).ToList();

            Console.WriteLine("  First 3 contacts:");
            foreach (var info in contactInfo)
            {
                Console.WriteLine($"    - {info.Name} ({info.Email ?? "No email"})");
            }

            Console.WriteLine("\nExample 4: FirstOrDefault() - Find a specific contact");
            var specificContact = results.FirstOrDefault(e =>
                e.Contains("emailaddress1") &&
                e.GetAttributeValue<string>("emailaddress1")?.Contains("example.com") == true);

            if (specificContact != null)
            {
                Console.WriteLine($"  Found contact: {specificContact.GetAttributeValue<string>("firstname")} " +
                                 $"{specificContact.GetAttributeValue<string>("lastname")} " +
                                 $"({specificContact.GetAttributeValue<string>("emailaddress1")})");
            }
            else
            {
                Console.WriteLine("  No contact found with example.com email address");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Advanced filtering examples
        /// </summary>
        private static void RunAdvancedFilterExample(IOrganizationService service)
        {
            Console.WriteLine("7. Advanced Filter Examples");
            Console.WriteLine("-------------------------");

            // Example for date and user filters
            var query = Query.For("task")
                .Select("subject", "description", "scheduledend", "ownerid")
                .Where(f => f
                    .Equal("statecode", 0)
                    .And(a => a
                        // Date operators
                        .OnOrAfter("scheduledend", DateTime.Today)
                    )
                    .And(a => a
                        // User operators (current user's tasks)
                        .EqualUserId("ownerid")
                    )
                )
                .OrderBy("scheduledend");

            Console.WriteLine("Query: Tasks due in the next 7 days assigned to current user");

            try
            {
                // Execute the query
                var results = query.RetrieveMultiple(service);

                // Display results
                Console.WriteLine($"Result: {results.Entities.Count} records found");
                foreach (var task in results.Entities)
                {
                    var dueDate = task.GetAttributeValue<DateTime>("scheduledend");
                    Console.WriteLine($"  - {task.GetAttributeValue<string>("subject")} " +
                                     $"(Due: {dueDate.ToShortDateString()})");
                }
            }
            catch (Exception ex)
            {
                // This might fail if current user doesn't have permissions
                Console.WriteLine($"  Note: This example could not be executed: {ex.Message}");
            }

            Console.WriteLine();
        }
    }
}