
using BusTrackingSystem.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BusTrackingSystem.API;

public class BusTrackingDbContextFactory
    : IDesignTimeDbContextFactory<BusTrackingDbContext>
{
    public BusTrackingDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "DefaultConnection was not found.");

        var optionsBuilder =
            new DbContextOptionsBuilder<BusTrackingDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new BusTrackingDbContext(optionsBuilder.Options);
    }
}
