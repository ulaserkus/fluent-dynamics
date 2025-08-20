# FluentDynamics QueryBuilder Roadmap

This document outlines the vision, guiding principles, planned features, and status for upcoming releases of FluentDynamics QueryBuilder.

## 1. Vision
Provide a fluent, expressive, and extensible API for crafting Microsoft Dynamics 365 / Dataverse queries that:
- Reduces boilerplate and error-prone manual QueryExpression code
- Encourages readable, testable composition of complex logic
- Scales from simple filters to advanced scenarios (aggregation, projections, cross‑cutting policies)
- Stays lightweight while offering opt‑in advanced capabilities

## 2. Guiding Principles
| Principle | Description |
|-----------|-------------|
| Ergonomics | Fluent chains are predictable; minimal surprises. |
| Performance | Avoid unnecessary allocations; clone only when needed. |
| Transparency | Output (QueryExpression / FetchXML) should be inspectable (e.g. DebugView). |
| Backward Compatibility | Deprecate with warnings before removals (major version boundaries). |
| Modularity | Advanced features should not bloat the core (consider package layering later). |

## 3. Status Legend
- ✅ Done
- 🚧 In Progress
- 📝 Planned
- ⏳ Under Evaluation / Idea
- ❌ Dropped (with rationale)

## 4. Version Tracks

### 1.1.x (Short Term)
| Item | Status | Notes |
|------|--------|-------|
| True async (IOrganizationServiceAsync2) + TopCount early break | 📝 Planned | Mark existing Task.Run wrappers `[Obsolete]` |
| CountAsync / ExistsAsync helpers | 📝 Planned | Lightweight performance / readability win |
| AddSelect / AndWhere incremental APIs | 📝 Planned | Preserve current `Select()` override semantics |
| DebugView() for structural inspection | 📝 Planned | Human-readable tree |
| Lightweight pagination clone | 📝 Planned | Avoid full DeepClone for page mutations |

### 1.2–1.3 (Mid Term)
| Item | Status | Notes |
|------|--------|-------|
| Interceptor pipeline (tenant/security enrichment) | ⏳ Under Evaluation | Finalize order semantics |
| Projection helpers (Project<T>) | ⏳ Under Evaluation | Null/alias safety |
| Reusable query templates (define/clone) | ⏳ Under Evaluation | Consistent base queries |
| OData query string generation (`ToODataQuery`) | ⏳ Under Evaluation | Partial operator coverage |
| Snapshot test harness (FetchXML or structure) | 📝 Planned | Regression protection |

### 2.0 (Long Term / Breaking)
| Item | Status | Notes |
|------|--------|-------|
| Remove pseudo-async wrappers | 📝 Planned | After deprecation window |
| Aggregation / Grouping DSL (FetchXML bridge) | ⏳ Under Evaluation | Requires broader test matrix |
| IAsyncEnumerable streaming support | ⏳ Under Evaluation | netstandard2.1+ / multi-target |
| Package partition (core / extensions / advanced) | ⏳ Under Evaluation | Keep base install lean |

## 5. Feature Buckets

### Ergonomics
- AddSelect (append-only columns)
- AndWhere / OrWhere (post-build filter augmentation)
- OrderByDescending sugar
- Root entity alias

### Performance
- Lightweight clone for paging
- TopCount-aware loop termination
- (Later) streaming enumeration

### Extensibility
- Interceptor pipeline
- Column masking / redaction policy
- Batch execution (ExecuteMultiple + throttling strategy)

### Developer Experience
- DebugView()
- Snapshot test infra
- BenchmarkDotNet suite (clone vs streaming vs all-pages)
- (Later) Source generator for strongly typed attribute access

### Data Output / Transformation
- Projection helpers (DTO mapping)
- Aliased value resolver improvements
- OData query string generator

### Advanced Query
- Aggregation DSL
- Subquery helpers (`WhereInSubquery(...)`)
- Exists / Count (initial simple helpers first)

## 6. Open Questions
| Question | Topic |
|----------|-------|
| Interceptor order resolution? | Global vs per-instance layering |
| Aggregation packaging? | Core vs separate `*.Aggregates` module |
| Source generator metadata source? | Build-time export vs live metadata |
| OData mapping limitations? | Documentation clarity & fallbacks |
| Multi-target strategy for streaming? | netstandard2.0 retention vs added TFs |

## 7. Deprecations
| API | Status | Removal Target | Notes |
|-----|--------|----------------|-------|
| Task.Run-based *Async methods | Planned (to be `[Obsolete]` in 1.1.x) | 2.0.0 | Replace with true async overloads |

## 8. Contribution Format
When proposing a feature, please use:
```
Title:
Problem:
Proposed API / Shape:
Example Usage:
Value:
Risks / Trade-offs:
Estimated Scope (S / M / L):
Priority (Low / Medium / High):
```

## 9. Relationship to CHANGELOG
Completed items move to CHANGELOG.md upon release. Roadmap focuses on future and in-flight planning.

## 10. Planned Changelog Bridge
CHANGELOG.md will be introduced with 1.1.0 (initial true async + helpers).

---

Last Updated: (2025-08-20)
