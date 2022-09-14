using JunkyardWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public class CarRepository : IRepository<Car>
{
    private readonly JunkyardContext _context;
    
    public CarRepository(JunkyardContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    public async Task<ICollection<Car>> Get()
    {
        return await _context.Cars.Include(c => c.AvailableParts).ToListAsync();
    }

    public async Task<Car>? GetById(int id)
    {
        return await _context.Cars.Include(c => c.AvailableParts).SingleOrDefaultAsync(c => c.CarId == id);
    }

    public async Task Add(Car car)
    {
        _context.Cars.Add(car);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Car car)
    {
        _context.Entry(car).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Car car)
    {
        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }
}