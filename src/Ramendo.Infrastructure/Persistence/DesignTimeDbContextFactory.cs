using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ramendo.Infrastructure.Persistence;

// Used only by dotnet-ef CLI (migrations add / database update).
// Never invoked at runtime.
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RamendoDbContext>
{
    public RamendoDbContext CreateDbContext(string[] args)
    {
        var connStr = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? "Host=localhost;Database=ramendo;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<RamendoDbContext>()
            .UseNpgsql(connStr)
            .UseSnakeCaseNamingConvention()
            .Options;

        return new RamendoDbContext(options);
    }
}
