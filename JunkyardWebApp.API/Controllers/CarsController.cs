using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;
using Microsoft.AspNetCore.Mvc;

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