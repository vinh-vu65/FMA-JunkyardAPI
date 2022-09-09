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
    public IActionResult GetAll()
    {
        return Ok(_context.Parts);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var part = _context.Parts.Find(id);
        if (part is null)
        {
            return NotFound();
        }
        return Ok(part);
    }

    [HttpPost]
    public IActionResult Add([FromBody]Part part)
    {
        _context.Add(part);
        _context.SaveChanges();

        return CreatedAtAction(
            "GetById",
            new {id = part.PartId},
            part);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody]Part part, int id)
    {
        if (id != part.PartId)
        {
            return BadRequest();
        }
        
        _context.Entry(part).State = EntityState.Modified;
        
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_context.Parts.Find(id) is null)
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<Part> Delete(int id)
    {
        var part = _context.Parts.Find(id);

        if (part is null)
        {
            return NotFound();
        }

        _context.Parts.Remove(part);
        _context.SaveChanges();

        return part;
    }
}