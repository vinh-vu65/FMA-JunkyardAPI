using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public interface IDbSeeder
{
    void SeedData(ModelBuilder modelBuilder);
}