using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JunkyardWebApp.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAll();
        var mappedCars = cars.Select(c => c.ToDtoV1());
        return Ok(mappedCars);
    }

    [HttpGet("{carId}")]
    public async Task<IActionResult> GetById(int carId)
    {
        var car = await _carService.GetById(carId);
        
        if (car is null)
        {
            return NotFound(new{ StatusCode = 404, Message = "Car not found" });
        }
        
        var carDto = car.ToDtoV1();
        return Ok(carDto);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]CarWriteDtoV1 requestData, int? id = null)
    {
        var car = new Car();
        if (id.HasValue)
        {
            var carAlreadyExists = await _carService.CarExistsInDb(id.Value);
            if (carAlreadyExists)
            {
                return BadRequest(new {StatusCode = 400, Message = "Another car already has this carId"});
            }
            car.CarId = id.Value;
        }
        
        car.UpdateWithV1(requestData);
        await _carService.Add(car);

        return CreatedAtAction(
            "GetById",
            new {carId = car.CarId},
            car);
    }

    [HttpPut("{carId}")]
    public async Task<IActionResult> Update([FromBody]CarWriteDtoV1 requestData, int carId)
    {
        var carToUpdate = await _carService.GetById(carId);
        if (carToUpdate is null)
        {
            return await Add(requestData, carId);
        }

        carToUpdate.UpdateWithV1(requestData);
        await _carService.Update(carToUpdate);
        
        return NoContent();
    }

    [HttpDelete("{carId}")]
    public async Task<IActionResult> Delete(int carId)
    {
        var car = await _carService.GetById(carId);

        if (car is null)
        {
            return NotFound(new{ StatusCode = 404, Message = "Car not found" });
        }

        await _carService.Delete(car);
        return NoContent();
    }
}