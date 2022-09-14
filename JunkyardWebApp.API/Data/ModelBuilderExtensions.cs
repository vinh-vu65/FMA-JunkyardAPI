using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>()
            .HasData(
                new Car { CarId = 1, Year = 2005, Make = "Toyota", Model = "Corolla" },
                new Car { CarId = 2, Year = 1995, Make = "Ford", Model = "Falcon" },
                new Car { CarId = 3, Year = 2012, Make = "Honda", Model = "Accord" },
                new Car { CarId = 4, Year = 2003, Make = "Nissan", Model = "Silvia" }
            );
        
        modelBuilder.Entity<Part>()
            .HasData(
                new Part { PartId = 1, Category = PartsCategory.Engine, Price = 1500.00M, Description = "Engine for 2005 Corolla", CarId = 1 },
                new Part { PartId = 2, Category = PartsCategory.Exhaust, Price = 700.00M, Description = "Exhaust for 2005 Corolla", CarId = 1 },
                new Part { PartId = 3, Category = PartsCategory.Door, Price = 200.00M, Description = "Front Passenger Door for 2005 Corolla", CarId = 1 },
                new Part { PartId = 4, Category = PartsCategory.Light, Price = 30.00M, Description = "Taillights for 2005 Corolla", CarId = 1 },
                new Part { PartId = 5, Category = PartsCategory.Wheel, Price = 450.00M, Description = "Wheels for 2005 Corolla", CarId = 1 },
                new Part { PartId = 6, Category = PartsCategory.Tyre, Price = 360.00M, Description = "Tyres for 1995 Falcon", CarId = 2 },
                new Part { PartId = 7, Category = PartsCategory.Brakes, Price = 820.00M, Description = "Brakes for 1995 Falcon", CarId = 2 },
                new Part { PartId = 8, Category = PartsCategory.Radiator, Price = 120.00M, Description = "Radiator for 1995 Falcon", CarId = 2 },
                new Part { PartId = 9, Category = PartsCategory.Suspension, Price = 300.00M, Description = "Suspension for 1995 Falcon", CarId = 2 },
                new Part { PartId = 10, Category = PartsCategory.Engine, Price = 1750.00M, Description = "Engine for 2012 Accord", CarId = 3 },
                new Part { PartId = 11, Category = PartsCategory.Light, Price = 55.00M, Description = "Brake light for 2012 Accord", CarId = 3 },
                new Part { PartId = 12, Category = PartsCategory.Door, Price = 190.00M, Description = "Rear driver door for 2012 Accord", CarId = 3 },
                new Part { PartId = 13, Category = PartsCategory.Exhaust, Price = 290.00M, Description = "Exhaust for 2003 Silvia", CarId = 4 },
                new Part { PartId = 14, Category = PartsCategory.Brakes, Price = 370.00M, Description = "Headlight for 2003 Silvia", CarId = 4 }
                );
        
        modelBuilder.Entity<Customer>()
            .HasData(
                new Customer { CustomerId = 1, Name = "Susan Boyle", Address = "123 Fake St", Email = "susan.boyle@gmail.com", Phone = "98573643"},
                new Customer { CustomerId = 2, Name = "Homer Simpson", Address = "742 Evergreen Tce", Email = "mmm_donuts@gmail.com", Phone = "59684938"}
                );
    }
}