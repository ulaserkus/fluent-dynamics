# FluentDynamics QueryBuilder

[![NuGet](https://img.shields.io/nuget/v/FluentDynamics.QueryBuilder.svg)](https://www.nuget.org/packages/FluentDynamics.QueryBuilder/)
[![License](https://img.shields.io/github/license/ulaserkus/fluent-dynamics)](https://github.com/ulaserkus/fluent-dynamics/blob/main/LICENSE)

A fluent interface for building and executing Microsoft Dynamics 365 queries. This library simplifies the process of creating complex QueryExpressions with a clean, chainable API.

## Installation

```bash
dotnet add package FluentDynamics.QueryBuilder
```

## Features

- Fluent builder for QueryExpressions
- Simplified syntax for conditions, joins, and filters
- Support for nested conditions with AND/OR operators
- Linq-like methods for handling query results
- Async/await support
- Pagination helpers
- FetchXML conversion

## Basic Usage

### Creating a Simple Query

```csharp
using FluentDynamics.QueryBuilder;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

// Create a connection to Dynamics 365 (using your preferred method)
IOrganizationService service = GetOrganizationService();

// Build a simple query
var query = new QueryExpressionBuilder("account")
    .Select("name", "accountnumber", "telephone1")
    .Where("statecode", ConditionOperator.Equal, 0)
    .OrderBy("name")
    .Top(10);

// Execute the query
EntityCollection results = query.Execute(service);

// Process the results
foreach (var entity in results.Entities)
{
    string name = entity.GetAttributeValue<string>("name");
    Console.WriteLine($"Account Name: {name}");
}
```

### Advanced Queries with Conditions

```csharp
var query = new QueryExpressionBuilder("contact")
    .Select("firstname", "lastname", "emailaddress1")
    .Where("statecode", ConditionOperator.Equal, 0)
    .And(q => q
        .Where("createdon", ConditionOperator.LastXDays, 30)
        .Where("emailaddress1", ConditionOperator.NotNull, null)
    )
    .Or(q => q
        .Where("lastname", ConditionOperator.BeginsWith, "Smith")
        .Where("lastname", ConditionOperator.BeginsWith, "Johnson")
    )
    .OrderBy("lastname")
    .OrderBy("firstname");

// Execute query
var results = query.Execute(service);

// Use extension methods
var contactList = results.ToList();
var activeEmails = results.Select(e => e.GetAttributeValue<string>("emailaddress1"));
```

### Joining Entities (Link Entities)

```csharp
var query = new QueryExpressionBuilder("opportunity")
    .Select("name", "estimatedvalue", "createdon")
    .Where("statecode", ConditionOperator.Equal, 0)
    .Link("account", "customerid", "accountid", JoinOperator.Inner, link => link
        .Select("name", "accountnumber")
        .Where("statecode", ConditionOperator.Equal, 0)
        .Link("contact", "primarycontactid", "contactid", JoinOperator.Inner, contactLink => contactLink
            .Select("firstname", "lastname", "emailaddress1")
            .Where("statecode", ConditionOperator.Equal, 0)
        )
    )
    .OrderBy("estimatedvalue", OrderType.Descending);

var results = query.Execute(service);
```

### Pagination

```csharp
// Using pagination
int pageSize = 50;
int pageNumber = 1;

var query = new QueryExpressionBuilder("account")
    .Select("name", "accountnumber")
    .Where("statecode", ConditionOperator.Equal, 0)
    .OrderBy("name");

// Get specific page
var page = query.ExecuteWithPagination(service, pageNumber, pageSize);

// Retrieve all pages automatically
var allResults = query.ExecuteAllPages(service);
```

### Async Execution

```csharp
// Async execution
var query = new QueryExpressionBuilder("lead")
    .Select("firstname", "lastname", "companyname")
    .Where("statecode", ConditionOperator.Equal, 0);

// Execute asynchronously
var results = await query.ExecuteAsync(service);

// With pagination
var pageResults = await query.ExecuteWithPaginationAsync(service, 1, 50);

// All pages asynchronously
var allResults = await query.ExecuteAllPagesAsync(service);
```

### Using Extensions for EntityCollection

```csharp
var query = new QueryExpressionBuilder("contact")
    .SelectAll()
    .Where("statecode", ConditionOperator.Equal, 0);

var results = query.Execute(service);

// Convert to list
var contactList = results.ToList();

// Convert to typed list
var typedList = results.ToTypedList<Entity>();

// Get first match
var contact = results.FirstOrDefault(e => e.GetAttributeValue<string>("lastname") == "Smith");

// Filter results in memory
var filtered = results.Where(e => e.Contains("emailaddress1")).ToList();

// Project to specific values
var emails = results.Select(e => e.GetAttributeValue<string>("emailaddress1")).ToList();
```

### Converting to QueryExpression or FetchXML

```csharp
var builder = new QueryExpressionBuilder("account")
    .Select("name", "accountnumber")
    .Where("statecode", ConditionOperator.Equal, 0);

// Get the underlying QueryExpression
QueryExpression queryExpression = builder.ToQueryExpression();

// Convert to FetchXML
FetchExpression fetchExpression = builder.ToFetchExpression(service);
```

### Using TryGet for Safe Attribute Access

```csharp
var query = new QueryExpressionBuilder("contact")
    .Select("firstname", "lastname", "emailaddress1")
    .Where("statecode", ConditionOperator.Equal, 0);

var results = query.Execute(service);

foreach (var entity in results.Entities)
{
    // Safely get attributes with fallback value
    string email = entity.TryGet<string>("emailaddress1", "No Email");
    DateTime? birthdate = entity.TryGet<DateTime?>("birthdate");
    
    Console.WriteLine($"Email: {email}, Birthdate: {birthdate}");
}
```

### Cloning Queries

```csharp
// Create a base query
var baseQuery = new QueryExpressionBuilder("account")
    .Select("name", "accountnumber")
    .Where("statecode", ConditionOperator.Equal, 0);

// Clone and modify for a specific use case
var activeAccountsQuery = baseQuery.Clone()
    .Where("createdon", ConditionOperator.LastXDays, 30);

// Clone and modify for another use case
var premiumAccountsQuery = baseQuery.Clone()
    .Where("accountcategorycode", ConditionOperator.Equal, 1);
```

## License

MIT License
