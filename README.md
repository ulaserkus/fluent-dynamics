# FluentDynamics QueryBuilder

FluentDynamics QueryBuilder is a fluent, chainable API for building and executing Dynamics 365/Dataverse queries. It simplifies the process of creating complex QueryExpressions with a more intuitive and readable syntax.

[![NuGet](https://img.shields.io/nuget/v/FluentDynamics.QueryBuilder.svg)](https://www.nuget.org/packages/FluentDynamics.QueryBuilder/)
[![License](https://img.shields.io/github/license/ulaserkus/fluent-dynamics)](LICENSE)

## Features

- 🔄 **Fluent API** - Chainable, intuitive query building
- 🔍 **Type-safe** - Strong typing for Dynamics 365 operations
- 🚀 **Async Support** - Full support for async/await patterns
- 📊 **LINQ-like Operations** - Familiar extension methods for query results
- 📑 **Pagination** - Built-in support for handling paged results
- 🔗 **Complex Joins** - Easily create and configure link-entity operations
- 🧩 **Extensible** - Clean architecture for extending functionality

## Installation

Install via NuGet Package Manager:

```
Install-Package FluentDynamics.QueryBuilder
```

Or via .NET CLI:

```
dotnet add package FluentDynamics.QueryBuilder
```

## Basic Usage

```csharp
// Import the namespace
using FluentDynamics.QueryBuilder;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

// Create a simple query
var query = Query.For("account")
    .Select("name", "accountnumber", "telephone1")
    .Where("statecode", ConditionOperator.Equal, 0)
    .OrderBy("name");

// Execute the query
EntityCollection results = query.RetrieveMultiple(organizationService);

// Use extension methods on results
var accounts = results.ToList();
```

## Advanced Examples

### Complex Filtering

```csharp
var query = Query.For("contact")
    .Select("firstname", "lastname", "emailaddress1")
    .Where("statecode", ConditionOperator.Equal, 0)
    .And(q => {
        q.Where("createdon", ConditionOperator.LastXDays, 30);
        q.Where("emailaddress1", ConditionOperator.NotNull);
    })
    .Or(q => {
        q.Where("parentcustomerid", ConditionOperator.Equal, accountId);
        q.Where("address1_city", ConditionOperator.Equal, "Seattle");
    })
    .OrderBy("lastname")
    .OrderBy("firstname");
```

### Joining Entities (Link Entities)

```csharp
var query = Query.For("opportunity")
    .Select("name", "estimatedvalue", "closeprobability")
    .Where("statecode", ConditionOperator.Equal, 0)
    .Link("account", "customerid", "accountid", JoinOperator.Inner, link => {
        link.Select("name", "accountnumber")
            .As("account")
            .Where("statecode", ConditionOperator.Equal, 0);
    })
    .Link("contact", "customerid", "contactid", JoinOperator.LeftOuter, link => {
        link.Select("fullname", "emailaddress1")
            .As("contact");
    });
```

### Pagination

```csharp
// Get a specific page
var page2 = query.RetrieveMultiple(service, pageNumber: 2, pageSize: 50);

// Retrieve all pages automatically
var allResults = query.RetrieveMultipleAllPages(service);

// Using async version
var results = await query.RetrieveMultipleAsync(service);
```

### Working with Results

```csharp
// Convert to list
var entities = results.ToList();

// Filter results
var filteredEntities = results.Where(e => e.Contains("emailaddress1"));

// Project to new form
var names = results.Select(e => e.GetAttributeValue<string>("name"));

// Get first matching entity
var matchingContact = results.FirstOrDefault(e => 
    e.GetAttributeValue<string>("emailaddress1").Contains("example.com"));

// Safe attribute access
string name = entity.TryGet<string>("name", "Default Name");
```

## API Reference

### Query

The entry point for building queries:
- `Query.For(entityName)` - Creates a new query for the specified entity

### QueryExpressionBuilder

Methods for configuring the main query:
- `Select(attributes)` - Specifies columns to include
- `SelectAll()` - Includes all columns
- `Where(attribute, operator, value)` - Adds a filter condition
- `And(builder)` - Adds a nested AND filter group
- `Or(builder)` - Adds a nested OR filter group
- `OrderBy(attribute, [orderType])` - Adds a sort order
- `Link(toEntity, fromAttribute, toAttribute, joinType, builder)` - Adds a join
- `Top(count)` - Limits the number of records
- `Distinct()` - Returns only distinct records
- `NoLock()` - Uses NOLOCK hint
- `QueryHint(hint)` - Adds a query hint
- `ForceSeek(indexName)` - Forces using a specific index

### Execution Methods

- `RetrieveMultiple(service)` - Executes the query
- `RetrieveMultiple(service, pageNumber, pageSize)` - Executes with pagination
- `RetrieveMultipleAllPages(service)` - Retrieves all pages
- `RetrieveMultipleAsync(service)` - Async version
- `ToQueryExpression()` - Converts to QueryExpression
- `ToFetchExpression(service)` - Converts to FetchXML

### Extension Methods

- `ToList()` / `ToArray()` - Convert results to collection types
- `FirstOrDefault(predicate)` - Returns first matching entity
- `SingleOrDefault(predicate)` - Returns single matching entity
- `Where(predicate)` - Filters entities
- `Select(selector)` - Projects entities to new form
- `TryGet<T>(attributeName, defaultValue)` - Safely gets attribute value

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
