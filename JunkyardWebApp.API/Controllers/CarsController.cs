using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly JunkyardContext _context;

    public CarsController(JunkyardContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _context.Cars.ToArrayAsync();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car is null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Car car)
    {
        _context.Add(car);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            "GetById",
            new {id = car.CarId},
            car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]Car car, int id)
    {
        if (id != car.CarId)
        {
            return BadRequest();
        }
        
        _context.Entry(car).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _context.Cars.FindAsync(id) is null)
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Car>> Delete(int id)
    {
        var car = await _context.Cars.FindAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        return car;
    }
}