using BusTrackingSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BusTrackingSystem.API.Data;

public class BusTrackingDbContext: DbContext
{
    public BusTrackingDbContext(
        DbContextOptions<BusTrackingDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Bus> Buses => Set<Bus>();
    public DbSet<Location> Locations => Set<Location>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Bus>()
        .HasIndex(b=>b.BusNumber)
        .IsUnique();
        modelBuilder.Entity<Bus>()
        .HasMany(b=>b.Locations)
        .WithOne(l=>l.Bus)
        .HasForeignKey(l=>l.BusId)
        .OnDelete(DeleteBehavior.Cascade);
    }
    
}