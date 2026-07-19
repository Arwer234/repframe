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

### Security & authentication

- Cookie-based ASP.NET Core Identity (same-origin SPA + API)
- Resource-based authorization per user
- See [Authentication & Authorization ADR](docs/adr/authentication-and-authorization.md) for architectural decisions.

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
│  └─ api/                # ASP.NET Core API
├─
```

## Product Principles

- **Training-first UX** — logging a set should be fast and reliable during a real workout
- **Progress over vanity metrics** — data should support decisions, not create noise
- **Real data before automation** — build reliable logging and clear trends before adding AI
- **Explicit trade-offs** — add technologies such as Redis, events, or real-time updates only when they solve a demonstrated problem
- **Accessible by default** — forms, navigation, validation, and feedback should work with keyboard and assistive technology
- **Mobile-first** — the active workout flow is designed for use at the gym

## Roadmap

- [ ] Slice 001: Log a working set
- [ ] Slice 002: Display the last working set for an exercise
- [ ] Slice 003: Edit and delete logged sets
- [ ] Workout sessions and reusable templates
- [ ] Authentication and per-user data isolation
- [ ] Exercise history, PRs, and e1RM trends
- [ ] Volume and workload dashboard
- [ ] Body-weight tracking and weekly check-ins
- [ ] Coach-ready progress reports and data export
- [ ] Optional cache and domain events for read-model updates
- [ ] Optional AI-assisted interpretation of accumulated data
