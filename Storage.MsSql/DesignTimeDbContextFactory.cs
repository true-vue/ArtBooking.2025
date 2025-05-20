using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Storage.MsSql;

/// <summary>
/// Enables EF migrations for this project separatly.
/// https://learn.microsoft.com/en-us/ef/core/cli/dbcontext-creation?tabs=dotnet-core-cli#from-a-design-time-factory
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ArtBookingDbContextMsSql>
{
    public ArtBookingDbContextMsSql CreateDbContext(string[] args)
    {
        // Set the base path for the configuration builder
        var basePath = Directory.GetCurrentDirectory();
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .Build();

        var builder = new DbContextOptionsBuilder<ArtBookingDbContextMsSql>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        builder.UseSqlServer(connectionString);

        return new ArtBookingDbContextMsSql(builder.Options);
    }
}