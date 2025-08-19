using FluentDynamics.QueryBuilder;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

string clientId = "<client-id>";
string clientSecret = "<client-secret>";
Uri serviceUri = new Uri("<org-url>");
string connectionString = $"AuthType=ClientSecret;Url={serviceUri};ClientId={clientId};ClientSecret={clientSecret};";
IOrganizationService service = new ServiceClient(connectionString);


var results = await Query.For("account")
    .Select("name", "accountnumber", "telephone1")
    .Where("statecode", ConditionOperator.Equal, 0)
    .OrderBy("name")
    .Top(10)
    .RetrieveMultiple(service)
    .ToListAsync();



Console.WriteLine("EOP");



