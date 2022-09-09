using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Models.Data;

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
            .HasForeignKey(c => c.CarId);
        
        modelBuilder.Entity<Customer>()
            .HasMany(c => c.OrderHistory)
            .WithOne(o => o.Customer)
            .HasForeignKey(c => c.CustomerId);
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems);
        
        modelBuilder.Entity<Order>()
            .HasOne(c => c.Customer);
        
        modelBuilder.Entity<Part>()
            .HasOne(p => p.Car);

        modelBuilder.Seed();
    }
}