using FluentDynamics.QueryBuilder;
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

            var results = Query.For("account")
                .Select("name", "accountnumber", "telephone1")
                .Where("statecode", ConditionOperator.Equal, 0)
                .OrderBy("name")
                .Top(10)
                .RetrieveMultiple(service)
                .ToList();


            Console.WriteLine("EOP");
        }
    }
}
