using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RepFrame.Api;

/// <summary>
/// EF Core design-time context factory for migrations (dotnet ef commands).
///
/// Local dev:
///   dotnet ef migrations add Foo -- --ConnectionStrings:DefaultConnection "Host=...;Port=5432;Database=...;Username=...;Password=..."
///   or
///   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=repframe;Username=postgres;Password=..."
///
/// GitHub Actions CI:
///   env:
///     ConnectionStrings:DefaultConnection: "Host=localhost;Port=5432;Database=repframe;Username=postgres;Password=${{ secrets.POSTGRES_PASSWORD }}"
/// </summary>
public class RepFrameDbContextFactory : IDesignTimeDbContextFactory<RepFrameDbContext>
{
    public RepFrameDbContext CreateDbContext(string[] args)
    {
        // Build configuration from appsettings.json and user-secrets
        var basePath = Environment.CurrentDirectory;
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile(Path.Combine(basePath, "appsettings.json"))
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found. " +
                "Set it in appsettings.json, user-secrets, or environment variables.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<RepFrameDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new RepFrameDbContext(optionsBuilder.Options);
    }
}
