using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly IRepository<Car> _carRepository;

    public CarsController(IRepository<Car> carRepository)
    {
        _carRepository = carRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carRepository.Get();
        return Ok(cars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _carRepository.GetById(id);
        if (car is null)
        {
            return NotFound();
        }
        return Ok(car);
    }

    [HttpGet("{id}/parts")]
    public async Task<IActionResult> GetPartsByCar(int id)
    {
        var car = await _carRepository.GetById(id);
    
        if (car is null)
        {
            return NotFound();
        }
        
        var carParts = car.AvailableParts;
        return Ok(carParts);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Car car)
    {
        await _carRepository.Add(car);

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
        
        if (await _carRepository.GetById(id) is null)
        {
            return NotFound();
        }
        
        await _carRepository.Update(car);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _carRepository.GetById(id);

        if (car is null)
        {
            return NotFound();
        }

        await _carRepository.Delete(car);

        return NoContent();
    }
}