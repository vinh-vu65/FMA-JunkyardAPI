using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly IService<Car> _carService;

    public CarsController(IService<Car> carService)
    {
        _carService = carService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAll();
        var mappedCars = cars.Select(c => c.ToDto());
        return Ok(mappedCars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _carService.GetById(id);
        
        if (car is null)
        {
            return NotFound(new{ StatusCode = 404, Message = "Car not found" });
        }
        
        var carDto = car.ToDto();
        return Ok(carDto);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]CarWriteDto requestData, int? id)
    {
        var car = new Car();
        if (id.HasValue)
        {
            car.CarId = id.Value;
        }
        
        car.UpdateWith(requestData);
        await _carService.Add(car);

        return CreatedAtAction(
            "GetById",
            new {id = car.CarId},
            car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]CarWriteDto requestData, int id)
    {
        var carToUpdate = await _carService.GetById(id);
        if (carToUpdate is null)
        {
            return await Add(requestData, id);
        }

        carToUpdate.UpdateWith(requestData);
        await _carService.Update(carToUpdate);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _carService.GetById(id);

        if (car is null)
        {
            return NotFound(new{ StatusCode = 404, Message = "Car not found" });
        }

        await _carService.Delete(car);
        return NoContent();
    }
}