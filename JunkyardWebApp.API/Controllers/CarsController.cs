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
    public async Task<IActionResult> Add([FromBody]PostPutCarDto carRequest)
    {
        var car = carRequest.ToEntity();
        await _carRepository.Add(car);

        return CreatedAtAction(
            "GetById",
            new {id = car.CarId},
            car);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]PostPutCarDto requestData, int id)
    {
        var carToUpdate = await _carRepository.GetById(id);
        if (carToUpdate is null)
        {
            return NotFound();
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