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
    public IActionResult GetAll()
    {
        return Ok(_context.Cars);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var car = _context.Cars.Find(id);
        if (car is null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    [HttpPost]
    public IActionResult Add([FromBody]Car car)
    {
        _context.Add(car);
        _context.SaveChanges();

        return CreatedAtAction(
            "GetById",
            new {id = car.CarId},
            car);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromBody]Car car, int id)
    {
        if (id != car.CarId)
        {
            return BadRequest();
        }
        
        _context.Entry(car).State = EntityState.Modified;
        
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (_context.Cars.Find(id) is null)
            {
                return NotFound();
            }
            
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult<Car> Delete(int id)
    {
        var car = _context.Cars.Find(id);

        if (car is null)
        {
            return NotFound();
        }

        _context.Cars.Remove(car);
        _context.SaveChanges();

        return car;
    }
}