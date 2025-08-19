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
- 🛠 **FetchXML Conversion** - Convert queries to FetchXML easily
- 🧮 **Distinct, NoLock, QueryHint, ForceSeek** - Advanced query options

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
using FluentDynamics.QueryBuilder;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

// Create a simple query
var query = Query.For("account")
    .Select("name", "accountnumber", "telephone1")
    .Where(f => f
        .Condition("statecode", ConditionOperator.Equal, 0)
    )
    .OrderBy("name");

// Execute the query
EntityCollection results = query.RetrieveMultiple(organizationService);

// Use extension methods on results
var accounts = results.ToList();
```

## Advanced Examples

### Complex Filtering (Nested AND/OR)

```csharp
var query = Query.For("contact")
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
```

### Joining Entities (Link Entities)

```csharp
var query = Query.For("opportunity")
    .Select("name", "estimatedvalue", "closeprobability")
    .Where(f => f
        .Condition("statecode", ConditionOperator.Equal, 0)
    )
    .Link("account", "customerid", "accountid", JoinOperator.Inner, link => {
        link.Select("name", "accountnumber")
            .As("account")
            .Where(f => f
                .Condition("statecode", ConditionOperator.Equal, 0)
            );
    })
    .Link("contact", "customerid", "contactid", JoinOperator.LeftOuter, link => {
        link.Select("fullname", "emailaddress1")
            .As("contact");
    });
```

### Pagination & Async

```csharp
// Get a specific page
var page2 = query.RetrieveMultiple(service, pageNumber: 2, pageSize: 50);

// Retrieve all pages automatically
var allResults = query.RetrieveMultipleAllPages(service);

// Using async version
var results = await query.RetrieveMultipleAsync(service);

// Async with pagination
var pageResults = await query.RetrieveMultipleAsync(service, pageNumber: 2, pageSize: 50);

// Async all pages
var allAsyncResults = await query.RetrieveMultipleAllPagesAsync(service);
```

### FetchXML Conversion

```csharp
// Convert QueryExpression to FetchXML
var fetchXml = query.ToFetchExpression(service);
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
    e.GetAttributeValue<string>("emailaddress1")?.Contains("example.com") == true);

// Safe attribute access
string name = entity.TryGet<string>("name", "Default Name");
```

## API Reference

### Query

Entry point for building queries:
- `Query.For(entityName)` - Creates a new query for the specified entity

### QueryExpressionBuilder

Methods for configuring the main query:
- `Select(params string[] attributes)` - Specifies columns to include
- `SelectAll()` - Includes all columns
- `Where(Action<FilterBuilder> filterConfig)` - Adds a filter group using fluent configuration
- `OrderBy(attribute, [orderType])` - Adds a sort order
- `Link(toEntity, fromAttribute, toAttribute, joinType, Action<LinkEntityBuilder> linkBuilder)` - Adds a join
- `Top(count)` - Limits the number of records
- `Distinct()` - Returns only distinct records
- `NoLock()` - Uses NOLOCK hint
- `QueryHint(hint)` - Adds a query hint
- `ForceSeek(indexName)` - Forces using a specific index

### Execution Methods

- `RetrieveMultiple(service)` - Executes the query and returns results
- `RetrieveMultiple(service, pageNumber, pageSize)` - Executes with pagination
- `RetrieveMultipleAllPages(service)` - Retrieves all pages synchronously
- `RetrieveMultipleAsync(service, CancellationToken cancellationToken = default)` - Async version
- `RetrieveMultipleAsync(service, pageNumber, pageSize, CancellationToken cancellationToken = default)` - Async with pagination
- `RetrieveMultipleAllPagesAsync(service, CancellationToken cancellationToken = default)` - Async all pages
- `ToQueryExpression()` - Converts to QueryExpression
- `ToFetchExpression(service)` - Converts to FetchXML

### FilterBuilder

Builds complex filter logic:
- `Condition(attribute, operator, value)` - Adds a condition
- `And(Action<FilterBuilder> nested)` - Adds a nested AND filter group
- `Or(Action<FilterBuilder> nested)` - Adds a nested OR filter group
- `ToExpression()` - Returns the built FilterExpression

### LinkEntityBuilder

Configures join/link entities:
- `Select(params string[] attributes)` - Specifies columns to include from the linked entity
- `SelectAll()` - Includes all columns from the linked entity
- `As(alias)` - Sets an alias for the linked entity
- `OrderBy(attribute, [orderType])` - Adds sort order to the linked entity
- `Where(Action<FilterBuilder> filterConfig)` - Configures link entity filters using a FilterBuilder
- `Link(toEntity, fromAttribute, toAttribute, joinType, Action<LinkEntityBuilder> linkBuilder)` - Adds a nested link-entity (join)

### Extension Methods (LINQ-like)

- `ToList()` / `ToArray()` - Convert results to collection types
- `FirstOrDefault(predicate)` - Returns first matching entity
- `SingleOrDefault(predicate)` - Returns single matching entity
- `Where(predicate)` - Filters entities
- `Select(selector)` - Projects entities to new form
- `TryGet<T>(attributeName, defaultValue)` - Safely gets attribute value
- `Clone()` - Deep clone of a query builder instance
---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
