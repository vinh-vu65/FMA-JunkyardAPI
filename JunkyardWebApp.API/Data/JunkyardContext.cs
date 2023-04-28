using JunkyardWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public class JunkyardContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Part> Parts { get; set; }
    private readonly IDbSeeder _dbSeeder;

    public JunkyardContext(DbContextOptions<JunkyardContext> options, IDbSeeder dbSeeder) : base(options)
    {
        _dbSeeder = dbSeeder;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasMany(c => c.AvailableParts)
            .WithOne(p => p.Car)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);

        _dbSeeder.SeedData(modelBuilder);
    }
}