using JunkyardWebApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Data;

public class PartRepository : IRepository<Part>
{
    private readonly JunkyardContext _context;
    
    public PartRepository(JunkyardContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Part>> Get()
    {
        return await _context.Parts.ToListAsync();
    }

    public async Task<Part>? GetById(int id)
    {
        return await _context.Parts.FindAsync(id);
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