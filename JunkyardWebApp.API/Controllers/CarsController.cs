using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
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
        var mappedCars = cars.Select(c => c.ToDto());
        return Ok(mappedCars);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var car = await _carRepository.GetById(id);
        
        if (car is null)
        {
            return NotFound();
        }
        
        var carDto = car.ToDto();
        return Ok(carDto);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]PostPutCarDto requestData)
    {
        var car = new Car();
        car.UpdateWith(requestData);
        await _carRepository.Add(car);

        return CreatedAtAction(
            "GetById",
            new {id = car.CarId},
            car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]PostPutCarDto requestData, int id)
    {
        // ModelState.IsValid
        
        var carToUpdate = await _carRepository.GetById(id);
        if (carToUpdate is null)
        {
            var car = new Car {CarId = id};
            car.UpdateWith(requestData);
            await _carRepository.Add(car);
            
            return CreatedAtAction(
                "GetById",
                new {id = car.CarId},
                car);
        }

        carToUpdate.UpdateWith(requestData);
        await _carRepository.Update(carToUpdate);
        
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