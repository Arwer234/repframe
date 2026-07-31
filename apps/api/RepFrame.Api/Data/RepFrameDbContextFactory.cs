using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RepFrame.Api;

public class RepFrameDbContextFactory : IDesignTimeDbContextFactory<RepFrameDbContext>
{
    public RepFrameDbContext CreateDbContext(string[] args)
    {
        // Design-time connection string for EF Core migrations.
        // Production connection strings are managed via appsettings.json and environment variables.
        var connectionString = "Server=localhost;Database=repframe;TrustedConnection=true;TrustServerCertificate=true";

        var optionsBuilder = new DbContextOptionsBuilder<RepFrameDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new RepFrameDbContext(optionsBuilder.Options);
    }
}
