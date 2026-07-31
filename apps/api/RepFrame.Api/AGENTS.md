# RepFrame.Api — Agent Guidelines

## Central Package Management

All NuGet package versions are managed centrally in `Directory.Packages.props` at the root of the `apps/api` folder. When adding or updating dependencies:

- Define the package version in `Directory.Packages.props` under the appropriate `<PackageVersion>` element.
- Reference the package by name only (without `Version` attribute) in `.csproj` files.
- Do not specify version numbers directly in project files.

## Local Development — PostgreSQL Connection String

The local development PostgreSQL connection string is stored in **dotnet user-secrets** under the key `ConnectionStrings:DefaultConnection`.

The `RepFrameDbContextFactory` reads this value at design-time for EF Core migration commands (`dotnet ef`).

## No Redundant Using Directives

Do not add `using` directives for namespaces that match the file's current namespace path.
