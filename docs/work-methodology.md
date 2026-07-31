# Work Methodology — Vertical Slices

## Philosophy

Repframe is built using a **vertical slice architecture** approach. Each feature or user story is implemented as a complete vertical cut through all layers (API, database, frontend) rather than horizontal layer-by-layer development.

### Why vertical slices?

- **Incremental value** — each slice delivers a working piece of functionality
- **Reduced coupling** — features are loosely coupled, easier to change independently
- **Faster feedback** — no "big bang" releases, continuous integration of real features
- **Parallelizable** — multiple developers can work on different slices without conflicts

## Decision delays (YAGNI)

We explicitly defer decisions that don't impact the current slice:

| Decision                      | Deferred until                            | Rationale                                                      |
| ----------------------------- | ----------------------------------------- | -------------------------------------------------------------- |
| `Exercise.muscleGroup` field  | Slice where muscle group analysis is used | Adds DB migration and API contract change for no current value |
| Authentication / multi-user   | Slice that requires user isolation        | Over-engineering for single-user seed scenario                 |
| Workout templates             | Slice that uses them                      | No frontend need yet, backend complexity unnecessary now       |
| Pagination on set lists       | Slice where dataset grows large           | All sets fit in memory for slice 001                           |
| Caching / Redis               | Slice with measurable performance needs   | Premature optimization                                         |
| Docker Compose infrastructure | When running environment is needed        | Local PostgreSQL via `dotnet ef` is sufficient for dev         |

## Multi-user production goal

Repframe is intended to evolve into a **multi-user, production-grade application**. This means:

- Domain models use types suitable for multi-user scenarios (`Guid` identifiers, `DateTimeOffset` timestamps)
- API contracts follow RESTful best practices (proper status codes, error formats, content negotiation)
- Database schema supports user isolation from the start (even if single-user initially)
- No hardcoding of "single-user" assumptions that would require breaking changes later

This does **not** mean we implement multi-user features early — it means our foundational choices don't block it.

## Architecture — Feature-Sliced Design (FSD)

Repframe uses **Feature-Sliced Design** as its primary architectural pattern. This is an intentional choice for a learning project: the goal is to understand FSD in a real-world .NET context before applying it at scale in production work.

### Architecture decision for small projects

For **small projects** (≤5 features, ≤10 endpoints), traditional **feature folders** are usually better than full FSD:

| Factor         | Feature-Sliced Design                            | Feature Folders                           |
| -------------- | ------------------------------------------------ | ----------------------------------------- |
| Best for       | Medium/large apps with complex domain boundaries | Small projects, prototypes, learning      |
| Overhead       | More conventions to learn and maintain           | Simpler structure, easier onboarding      |
| Cohesion       | Excellent — features are fully isolated          | Good enough — related code stays together |
| Learning value | High — teaches a production architecture         | Moderate — standard .NET pattern          |

**Rule of thumb:** If the project has ≤5 features and ≤10 endpoints, use feature folders. FSD's benefits (isolation, discoverability, refactoring safety) only justify its overhead when you have enough features to make shared folders unwieldy.

This project uses FSD **not because it's optimal for our current size**, but because the goal is learning — understanding how FSD works in practice so that at work you can evaluate whether it fits your production projects.

### What is Feature-Sliced Design?

FSD organizes code by **business feature** rather than technical layer. Each feature is a self-contained "slice" with all its concerns co-located:

```
Features/
  WorkoutSessions/          // One business feature = one folder
    Models.cs               // DTOs + domain types for this feature
    Endpoint.cs             // MapPost/MapGet registrations
    Handler.cs              // Business logic / service methods
    Tests/                  // Unit + integration tests
  Exercises/                // Next feature...
```

### Why FSD over traditional layered architecture?

| Aspect          | Layered (Models/, Services/, Endpoints/)        | Feature-Sliced                                       |
| --------------- | ----------------------------------------------- | ---------------------------------------------------- |
| Cohesion        | Related code scattered across layers            | All feature code in one folder                       |
| Coupling        | Every new endpoint touches Models/, Services/   | Features are independent modules                     |
| Discoverability | "Where is the logic for X?" — search everywhere | "Which feature handles X?" — navigate by name        |
| Refactoring     | Changes ripple across many files                | Changes stay within a feature folder                 |
| Learning value  | Standard .NET pattern (everyone knows it)       | **Teaches a modern architecture used in production** |

### FSD conventions for this project

- **`Features/` root** — every business capability gets its own subfolder
- **`Models.cs`** — request/response DTOs + domain types scoped to the feature (not dumped into a global `Models/`)
- **`Endpoint.cs`** — minimal API route definitions (`MapPost`, `MapGet`) for that feature only
- **`Handler.cs`** — business logic / service methods specific to the feature
- **`Validation/`** — FluentValidation validators scoped to the feature

### When FSD starts to show its limits

FSD works well up to ~20-30 features. Beyond that, you may need:

- **Shared slices** (`Shared/`) for cross-cutting concerns (auth middleware, error handling, common DTOs)
- **Layers slice** (`Layers/`) for infrastructure (database, external APIs, caching)
- **Grid slices** — a meta-layer organizing features into logical groups

This project will explore those patterns as the feature count grows.

## Slice lifecycle

1. **Backlog** — user story in `product-backlog.md`
2. **Story spec** — detailed spec in `docs/user-stories/NNN-<name>.md` covering: goal, context, acceptance criteria, API contract, domain model, response schemas, error handling
3. **Implementation** — full vertical slice inside a feature folder (models → endpoint → handler → tests)
4. **Review** — verify against acceptance criteria
