# FluentDynamics QueryBuilder

FluentDynamics QueryBuilder is a fluent, chainable API for building and executing Dynamics 365/Dataverse queries. It simplifies the process of creating complex QueryExpressions with a more intuitive and readable syntax.

[![NuGet](https://img.shields.io/nuget/v/FluentDynamics.QueryBuilder.svg)](https://www.nuget.org/packages/FluentDynamics.QueryBuilder/)
[![License](https://img.shields.io/github/license/ulaserkus/fluent-dynamics)](LICENSE)
![Line Coverage](https://img.shields.io/badge/line%20coverage-85.35%25-brightgreen)
![Branch Coverage](https://img.shields.io/badge/branch%20coverage-72.72%25-yellow)
![Method Coverage](https://img.shields.io/badge/method%20coverage-94.55%25-brightgreen)
![Tests](https://img.shields.io/badge/tests-180%20passed-brightgreen)

## Features

- 🔄 **Fluent API** - Chainable, intuitive query building
- 🔍 **Type-safe** - Strong typing for Dynamics 365 operations
- 🚀 **Async Support** - Full support for async/await patterns
- 📊 **LINQ-like Operations** - Familiar extension methods for query results
- 📑 **Pagination** - Built-in support for handling paged results
- 🔗 **Complex & Advanced Joins** - Rich set of join helpers (Inner, LeftOuter, Natural, CrossApply, In, Exists, Any/NotAny, All/NotAll)
- 🧩 **Extensible** - Clean architecture for extending functionality
- 🛠 **FetchXML Conversion** - Convert queries to FetchXML easily
- 🧮 **Distinct, NoLock, QueryHint, ForceSeek** - Advanced query options
- ⚡ **FilterBuilder Extensions** - Syntactic sugar methods for common conditions (Equal, In, Like, LastXDays, IsNull, etc.)
- 🔍 **Query Debugging** - Human-readable query inspection with DebugView
- 🔄 **Optimized Cloning** - Efficient query cloning operations for different use cases

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

### Complex Filtering Using FilterBuilder Extensions

```csharp
var query = Query.For("contact")
    .Select("firstname", "lastname", "emailaddress1")
    .Where(f => f
        .Equal("statecode", 0)
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

---

## Advanced Join Extensions

The library exposes helper extension methods (in `QueryBuilderExtensions`) for additional Dataverse `JoinOperator` values. These allow more expressive and often more performant queries in certain scenarios.

| Extension Method | Underlying JoinOperator | Purpose / Behavior |
|------------------|-------------------------|--------------------|
| `InnerJoin` | `Inner` | Standard inner join (matching related rows only) |
| `LeftOuterJoin` | `LeftOuter` | Include parent even if no related rows exist |
| `NaturalJoin` | `Natural` | Natural join (rarely used; relies on platform semantics) |
| `CrossApplyJoin` | `MatchFirstRowUsingCrossApply` | Optimizes when you only need the first matching child row |
| `InJoin` | `In` | Translates to `IN` semantics; can improve performance on large link sets |
| `ExistsJoin` | `Exists` | Uses `EXISTS` logic to restrict parents to those having matches |
| `AnyJoin` | `Any` | Parent rows where any related rows (after link filters) exist |
| `NotAnyJoin` | `NotAny` | Parent rows where no related rows (after link filters) exist |
| `AllJoin` | `All` | Parents with related rows where none of those rows satisfy additional filters (logical “ALL NOT”) |
| `NotAllJoin` | `NotAll` | Alias of `Any` semantics (Dataverse quirk) |

> Note: Some of these operators can significantly change the result semantics and/or execution plan. Always validate with real data and inspect the generated FetchXML (via `ToFetchExpression`) or use `DebugView()` for clarity.

### When to Use Which Advanced Join

- Use `CrossApplyJoin` to fetch a single "primary" child row (e.g., latest activity) without bringing an entire child set.
- Use `ExistsJoin` / `AnyJoin` when you want to check for the presence of related rows without needing their data.
- Use `NotAnyJoin` to enforce absence of related data (e.g. accounts without open opportunities).
- Use `InJoin` when a sub-select style inclusion might be faster than a traditional join (test vs inner join).
- Use `AllJoin` when you need parent rows where all related rows fail a condition expressed in the filters (Dataverse implements this as “all related but none meet filter X”).

### Code Examples

#### 1. Any vs NotAny (Presence / Absence)

```csharp
// Accounts that HAVE at least one active contact
var accountsWithActiveContact = Query.For("account")
    .AnyJoin("contact", "accountid", "parentcustomerid", link => link
        .Where(f => f.Equal("statecode", 0))
    );

// Accounts that have NO active contact
var accountsWithoutActiveContact = Query.For("account")
    .NotAnyJoin("contact", "accountid", "parentcustomerid", link => link
        .Where(f => f.Equal("statecode", 0))
    );
```

#### 2. Exists vs In

```csharp
// Accounts that have at least one open opportunity (using EXISTS)
var accountsWithOpportunitiesExists = Query.For("account")
    .ExistsJoin("opportunity", "accountid", "parentaccountid", link => link
        .Where(f => f.Equal("statecode", 0))
    );

// Accounts using IN-based restriction (platform may choose different plan)
var accountsWithOpportunitiesIn = Query.For("account")
    .InJoin("opportunity", "accountid", "parentaccountid", link => link
        .Where(f => f.Equal("statecode", 0))
    );
```

#### 3. CrossApply (First Matching Child)

```csharp
// Get account with only ONE (first) recent task (improves performance vs full child set)
var accountsWithOneTask = Query.For("account")
    .CrossApplyJoin("task", "accountid", "regardingobjectid", link => link
        .Select("subject", "scheduledend")
        .OrderByDesc("scheduledend")  // Ensure desired ordering for "first"
        .Top(1) // (Optional) If you later add Top to the link (manual pattern)
    );
```

> Dataverse’s cross-apply logic tries to retrieve just the first qualifying child row; ensure your sorting reflects the intended “first”.

#### 4. All / NotAll Semantics

```csharp
// Accounts where related opportunities exist, but NONE of those are still open
var accountsWhereAllOpportunitiesClosed = Query.For("account")
    .AllJoin("opportunity", "accountid", "parentaccountid", link => link
        .Where(f => f.Equal("statecode", 0)) // Filter defines "open" – ALL join returns parents where no linked rows satisfy this
    );

// NotAll is effectively Any (platform quirk)
var accountsWithAnyOpenOpportunity = Query.For("account")
    .NotAllJoin("opportunity", "accountid", "parentaccountid", link => link
        .Where(f => f.Equal("statecode", 0))
    );
```

#### 5. Combining Multiple Advanced Joins

```csharp
var complex = Query.For("account")
    .Select("name")
    .AnyJoin("contact", "accountid", "parentcustomerid", c => c
        .Where(f => f.Equal("statecode", 0))
        .Select("fullname")
    )
    .NotAnyJoin("opportunity", "accountid", "parentaccountid", o => o
        .Where(f => f.Equal("statecode", 0)) // Has no active opportunities
    )
    .CrossApplyJoin("task", "accountid", "regardingobjectid", t => t
        .Select("subject")
        .OrderByDesc("createdon")
    );
```

### Performance Tips

| Scenario | Recommended Join Helper | Rationale |
|----------|-------------------------|-----------|
| Need only first child row | `CrossApplyJoin` | Avoids pulling all children |
| Presence check only | `ExistsJoin` or `AnyJoin` | Clear semantic & often cheaper |
| Absence check | `NotAnyJoin` | Efficient anti-semi logic |
| None of children meet condition | `AllJoin` | Expresses universal negative |
| Explore optimizer difference | Compare `InnerJoin` vs `InJoin` | Sometimes IN variant changes plan |
| Filtering parent by complex related predicate | `ExistsJoin` | Moves predicate to EXISTS scope |

Use `DebugView()` during development:

```csharp
var debug = complex.DebugView();
Console.WriteLine(debug);
```

---

## Pagination & Async

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

## FetchXML Conversion

```csharp
// Convert QueryExpression to FetchXML
var fetchXml = query.ToFetchExpression(service);
```

## Working with Results

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
- `OrderBy(attribute)` - Adds ascending a sort order
- `OrderByDesc(attribute)` - Adds descending a sort order
- `Link(toEntity, fromAttribute, toAttribute, joinType, Action<LinkEntityBuilder> linkBuilder)` - Adds a join
- `Top(count)` - Limits the number of records
- `Distinct()` - Returns only distinct records
- `NoLock()` - Uses NOLOCK hint
- `QueryHint(hint)` - Adds a query hint
- `ForceSeek(indexName)` - Forces using a specific index

### Execution Methods
- `RetrieveMultiple(service)`
- `RetrieveMultiple(service, pageNumber, pageSize)`
- `RetrieveMultipleAllPages(service)`
- `Exists(service)`
- `RetrieveMultipleAsync(service, CancellationToken cancellationToken = default)`
- `RetrieveMultipleAsync(service, pageNumber, pageSize, CancellationToken cancellationToken = default)`
- `RetrieveMultipleAllPagesAsync(service, CancellationToken cancellationToken = default)`
- `ExistsAsync(service, CancellationToken cancellationToken = default)`
- `ToQueryExpression()`
- `ToFetchExpression(service)`

### FilterBuilder
Builds complex filter logic:
- `Condition(attribute, operator, value)`
- `And(Action<FilterBuilder> nested)`
- `Or(Action<FilterBuilder> nested)`
- `ToExpression()`

### FilterBuilder Extensions (Syntactic Sugar)
Convenience methods mapping to common `ConditionOperator` values. They improve readability and reduce verbosity.

| Extension | Purpose | Equivalent |
|-----------|---------|-----------|
| `Equal(attr, value)` | Equality | `Condition(attr, Equal, value)` |
| `NotEqual(attr, value)` | Inequality | `Condition(attr, NotEqual, value)` |
| `GreaterThan(attr, value)` | Greater than | `Condition(attr, GreaterThan, value)` |
| `GreaterEqual(attr, value)` | Greater or equal | `Condition(attr, GreaterEqual, value)` |
| `LessThan(attr, value)` | Less than | `Condition(attr, LessThan, value)` |
| `LessEqual(attr, value)` | Less or equal | `Condition(attr, LessEqual, value)` |
| `Like(attr, pattern)` | SQL-like pattern | `Condition(attr, Like, pattern)` |
| `NotLike(attr, pattern)` | Negated like | `Condition(attr, NotLike, pattern)` |
| `BeginsWith(attr, value)` | Prefix | `Condition(attr, BeginsWith, value)` |
| `EndsWith(attr, value)` | Suffix | `Condition(attr, EndsWith, value)` |
| `Contains(attr, value)` | Contains text | `Condition(attr, Contains, value)` |
| `In(attr, params values)` | In list | `Condition(attr, In, valuesArray)` |
| `NotIn(attr, params values)` | Not in list | `Condition(attr, NotIn, valuesArray)` |
| `Between(attr, from, to)` | Between range | `Condition(attr, Between, new[]{from,to})` |
| `NotBetween(attr, from, to)` | Not between | `Condition(attr, NotBetween, new[]{from,to})` |
| `IsNull(attr)` | Null check | `Condition(attr, Null, null)` |
| `IsNotNull(attr)` | Not null | `Condition(attr, NotNull, null)` |
| `LastXDays(attr, days)` | Relative date | `Condition(attr, LastXDays, days)` |
| `NextXDays(attr, days)` | Relative date | `Condition(attr, NextXDays, days)` |
| `LastXMonths(attr, months)` | Relative date | `Condition(attr, LastXMonths, months)` |
| `NextXMonths(attr, months)` | Relative date | `Condition(attr, NextXMonths, months)` |
| `LastXYears(attr, years)` | Relative date | `Condition(attr, LastXYears, years)` |
| `NextXYears(attr, years)` | Relative date | `Condition(attr, NextXYears, years)` |
| `On(attr, date)` | Specific date | `Condition(attr, On, date)` |
| `OnOrBefore(attr, date)` | On or before | `Condition(attr, OnOrBefore, date)` |
| `OnOrAfter(attr, date)` | On or after | `Condition(attr, OnOrAfter, date)` |

Example:

```csharp
var q = Query.For("contact")
    .Select("firstname", "lastname", "emailaddress1", "createdon")
    .Where(f => f
        .Equal("statecode", 0)
        .IsNotNull("emailaddress1")
        .LastXDays("createdon", 30)
        .Or(o => o
            .Like("emailaddress1", "%@example.com")
            .In("address1_city", "Seattle", "London", "Berlin")
        )
    );
```

### LinkEntityBuilder
Configures join/link entities:
- `Select(params string[] attributes)`
- `SelectAll()`
- `As(alias)`
- `OrderBy(attribute)`
- `OrderByDesc(attribute)`
- `Where(Action<FilterBuilder> filterConfig)`
- `Link(toEntity, fromAttribute, toAttribute, joinType, Action<LinkEntityBuilder> linkBuilder)`

### Advanced Join Helpers (QueryBuilderExtensions)
- `InnerJoin(...)`
- `LeftOuterJoin(...)`
- `NaturalJoin(...)`
- `CrossApplyJoin(...)`
- `InJoin(...)`
- `ExistsJoin(...)`
- `AnyJoin(...)`
- `NotAnyJoin(...)`
- `AllJoin(...)`
- `NotAllJoin(...)`

### Extension Methods (LINQ-like)
- `ToList()` / `ToArray()`
- `FirstOrDefault(predicate)`
- `SingleOrDefault(predicate)`
- `Where(predicate)`
- `Select(selector)`
- `TryGet<T>(attributeName, defaultValue)`
- `DeepClone()`
- `ShallowClone()`
- `CloneForPagination()`
- `DebugView()`

---

### Module Coverage
| Module | Line | Branch | Method |
|--------|------|--------|--------|
| FluentDynamics.QueryBuilder | 85.35% | 72.72% | 94.55% |

### Overall Coverage
| Metric | Line | Branch | Method |
|--------|------|--------|--------|
| Total | 85.35% | 72.72% | 94.55% |
| Average | 85.35% | 72.72% | 94.55% |

### Test Summary
- Total Tests: 180
- Failed: 0
- Succeeded: 180
- Skipped: 0
- Duration: 2.5s

---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

---
