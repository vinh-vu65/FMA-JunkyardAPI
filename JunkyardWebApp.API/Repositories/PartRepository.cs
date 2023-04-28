using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Repositories;

public class PartRepository : IPartRepository
{
    private readonly JunkyardContext _context;
    
    public PartRepository(JunkyardContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public async Task<ICollection<Part>> Get()
    {
        return await _context.Parts
            .Include(p => p.Car)
            .OrderBy(p => p.CarId)
            .ThenBy(p => p.PartId)
            .ToListAsync();
    }

    public async Task<ICollection<Part>> GetPartsByCarId(int carId)
    {
        var parts = await _context.Parts
            .Where(p => p.CarId == carId)
            .Include(p => p.Car)
            .OrderBy(p => p.PartId)
            .ToArrayAsync();
        
        return parts;
    }

    public async Task<Part?> GetById(int id)
    {
        return await _context.Parts
            .Include(p => p.Car)
            .SingleOrDefaultAsync(p => p.PartId == id);
    }

    public async Task Add(Part part)
    {
        _context.Parts.Add(part);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Part part)
    {
        _context.Entry(part).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Part part)
    {
        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();
    }
}