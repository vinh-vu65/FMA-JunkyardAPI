using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PartsController : ControllerBase
{
    private readonly JunkyardContext _context;

    public PartsController(JunkyardContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _context.Parts.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var part = await _context.Parts.FindAsync(id);
        if (part is null)
        {
            return NotFound();
        }
        return Ok(part);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Part part)
    {
        _context.Add(part);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            "GetById",
            new {id = part.PartId},
            part);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]Part part, int id)
    {
        if (id != part.PartId)
        {
            return BadRequest();
        }
        
        _context.Entry(part).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.Parts.FindAsync(id) is null)
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Part>> Delete(int id)
    {
        var part = await _context.Parts.FindAsync(id);

        if (part is null)
        {
            return NotFound();
        }

        _context.Parts.Remove(part);
        await _context.SaveChangesAsync();

        return part;
    }
}