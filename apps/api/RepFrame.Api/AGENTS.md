# RepFrame.Api — Agent Guidelines

## Central Package Management

All NuGet package versions are managed centrally in `Directory.Packages.props` at the root of the `apps/api` folder. When adding or updating dependencies:

- Define the package version in `Directory.Packages.props` under the appropriate `<PackageVersion>` element.
- Reference the package by name only (without `Version` attribute) in `.csproj` files.
- Do not specify version numbers directly in project files.

## Local Development — PostgreSQL Connection String

The local development PostgreSQL connection string is stored in **dotnet user-secrets** under the key `ConnectionStrings:DefaultConnection`.

The `RepFrameDbContextFactory` reads this value at design-time for EF Core migration commands (`dotnet ef`).

## Database Strategy — PostgreSQL (Production) + SQLite (Tests)

The project uses **PostgreSQL** as the production database and **SQLite** exclusively for unit tests.

### Implications for EF Core Queries

- All LINQ queries are written for **PostgreSQL compatibility**. PostgreSQL fully supports `DateTimeOffset` in ORDER BY, GROUP BY, and other advanced operations.
- SQLite has limited type support (no native `DateTimeOffset`, restricted ORDER BY expressions). **Never add workarounds like `.ToUniversalTime()`, `.ToUnixTimeSeconds()`, or `.AsEnumerable()` to production LINQ queries** — they are only needed when running against SQLite in tests, and would break PostgreSQL translation.
- If a query works with PostgreSQL's EF Core provider (`Npgsql.EntityFrameworkCore.PostgreSQL`), it will work in production. Tests use SQLite as an in-memory database for speed and isolation.

### Type Guidelines

- **Prefer `DateTime`** for all timestamps (e.g., `CreatedAt`, `StartedAt`, `FinishedAt`). The app logs times locally in a single timezone, so timezone awareness is not needed.
- Avoid `DateTimeOffset` — it adds complexity without benefit for this project and causes compatibility issues with SQLite in tests.
- Keep types consistent across models, DTOs, and handlers. Do not mix `DateTime` and `DateTimeOffset`.

## No Redundant Using Directives

Do not add `using` directives for namespaces that match the file's current namespace path.
