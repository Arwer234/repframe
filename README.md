# Repframe

> A strength training tracker for logging workouts, understanding progression, and making better training decisions from real data.

Repframe is a full-stack workout tracking application focused on strength training. It is being built as a portfolio and learning project to explore product-oriented full-stack development with React, ASP.NET Core, and PostgreSQL.

Repframe aims to make training data useful: record sets quickly during a session, compare performance over time, and surface meaningful signals around progression, volume, fatigue, and check-ins.

## Current Status

🚧 **Work in progress**

The project is currently in its first vertical slice: logging a workout set.

## Product Direction

Repframe is intended to evolve through small, production-like vertical slices.

### Workout logging

- Start a workout from a template or an empty session
- Log sets with weight, repetitions, RIR, notes, and set type
- View the previous performance for an exercise
- Copy, edit, or delete logged sets
- Use a rest timer during training
- Resume an unfinished workout after a refresh

### Plans and exercises

- Browse and create exercises
- Assign muscle groups to exercises
- Create reusable workout templates
- Reorder exercises in a template
- Add or skip exercises during an active workout

### Progress insights

- Review exercise history
- Track estimated 1RM trends
- See personal records
- Compare current and previous exercise performance
- Analyze working sets and tonnage by muscle group
- Compare weekly workload with recent averages

### Check-ins and fatigue

- Record body weight and body measurements
- Track fatigue, sleep quality, discomfort, and session notes
- Review weekly check-ins over time
- Surface non-diagnostic alerts for unusual performance or workload trends

## Tech Stack

### Frontend

- React
- TypeScript
- Vite
- React Router
- TanStack Query
- React Hook Form
- Zod
- Vitest and React Testing Library
- Playwright

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Npgsql
- OpenAPI / Swagger
- xUnit

### Tooling

- Docker Compose for local infrastructure
- GitHub Actions for CI
- ESLint and Prettier
- `dotnet format`
- Markdown-based product and architecture documentation

## Architecture

Repframe is a single-product monorepo:

```text
repframe/
├─ apps/
│  ├─ web/                # React application
│  └─ api/
│     ├─ RepFrame.Api/          # ASP.NET Core Web API
│     └─ RepFrame.Api.Tests/    # xUnit test project
├─ docs/
│  ├─ product-backlog.md
│  ├─ work-methodology.md
│  └─ user-stories/
└─ README.md
```

### Backend project structure

```text
apps/api/
├─ RepFrame.Api/
│  ├─ AGENTS.md              # Agent guidelines (architecture decisions, conventions)
│  ├─ Features/              # Feature-Sliced Design — one folder per business capability
│  │  ├─ Exercises/
│  │  │  ├─ ExerciseController.cs
│  │  │  ├─ ExerciseHandler.cs
│  │  │  ├─ ExerciseModels.cs
│  │  │  └─ Validation/
│  │  ├─ Sets/
│  │  │  ├─ SetController.cs
│  │  │  ├─ SetHandler.cs
│  │  │  ├─ SetModels.cs
│  │  │  └─ Validation/
│  │  └─ WorkoutSessions/
│  │     ├─ WorkoutSessionController.cs
│  │     ├─ WorkoutSessionHandler.cs
│  │     └─ WorkoutSessionModels.cs
│  ├─ Models/                # Shared domain models (entities)
│  ├─ Program.cs
│  └─ RepFrame.Api.csproj
└─ RepFrame.Api.Tests/
   ├─ Features/              # Tests organized by feature
   │  ├─ Exercises/
   │  ├─ Sets/
   │  └─ WorkoutSessions/
   ├─ RepFrame.Api.Tests.csproj
   └─ GlobalUsings.cs
```

### Feature folder naming convention

All files inside `Features/<FeatureName>/` use the **feature-prefixed pattern**:

- `SetHandler.cs` — not `Handler.cs`
- `ExerciseModels.cs` — not `Models.cs`
- `WorkoutSessionController.cs` — not `Controller.cs`

This makes every file uniquely identifiable in IDE navigation and git blame, regardless of how many feature folders exist.

See `apps/api/RepFrame.Api/AGENTS.md` for full conventions.

### Test project conventions

- **Framework:** xUnit with FluentAssertions and NSubstitute
- **Database testing:** `Microsoft.EntityFrameworkCore.InMemory` for isolated unit tests
- **Test organization:** Tests mirror the feature folder structure (`Features/<FeatureName>/`)
- **Isolation:** Each test class gets a fresh in-memory database via constructor-scoped `DbContext`
- **Naming:** `[MethodName]_Should_[ExpectedBehavior]` (e.g., `CreateAsync_ShouldPersistSet`)
