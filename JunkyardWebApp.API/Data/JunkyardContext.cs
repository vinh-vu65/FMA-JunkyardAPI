using JunkyardWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public class JunkyardContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Part> Parts { get; set; }

    public JunkyardContext(DbContextOptions<JunkyardContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasMany(c => c.AvailableParts)
            .WithOne(p => p.Car)
            .HasForeignKey(p => p.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // modelBuilder.Entity<Part>()
        //     .HasOne<Car>(p => p.Car)
        //     .WithMany(c => c.AvailableParts)
        //     .HasForeignKey(p => p.CarId);

        modelBuilder.Seed();
    }
}