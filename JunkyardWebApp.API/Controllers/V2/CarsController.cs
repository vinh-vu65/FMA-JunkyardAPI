using System.Net;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JunkyardWebApp.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }
    
    /// <summary>
    /// Retrieves all the cars in the database
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Successfully retrieves all the cars</response>
    [HttpGet]
    [ProducesResponseType(typeof(CarReadDtoV2), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var cars = await _carService.GetAll();
        var mappedCars = cars.Select(c => c.ToDtoV2());
        return Ok(mappedCars);
    }

    /// <summary>
    /// Retrieves a specific car
    /// </summary>
    /// <param name="carId"></param>
    /// <returns></returns>
    /// <response code="200">Successfully retrieves the car with given ID</response>
    /// <response code="404">Car with given ID does not exist</response>
    [HttpGet("{carId}")]
    [ProducesResponseType(typeof(CarReadDtoV2), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int carId)
    {
        var car = await _carService.GetById(carId);
        
        if (car is null)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }
        
        var carDto = car.ToDtoV2();
        return Ok(carDto);
    }

    /// <summary>
    /// Creates a new car entry
    /// </summary>
    /// <response code="201">Successfully created a new entry</response>
    /// <response code="400">If an ID was given as a query parameter, a car with this ID already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(CarReadDtoV2), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Add([FromBody]CarWriteDtoV2 requestData, int? id = null)
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
        
        car.UpdateWithV2(requestData);
        await _carService.Add(car);

        return CreatedAtAction(
            "GetById",
            new {carId = car.CarId},
            car);
    }

    /// <summary>
    /// Updates an entry with given ID
    /// </summary>
    /// <response code="201">Given ID did not yet exist, successfully created new entry</response>
    /// <response code="204">Successfully updated entry at given ID</response>
    [HttpPut("{carId}")]
    [ProducesResponseType(typeof(CarReadDtoV2), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> Update([FromBody]CarWriteDtoV2 requestData, int carId)
    {
        var carToUpdate = await _carService.GetById(carId);
        if (carToUpdate is null)
        {
            return await Add(requestData, carId);
        }

        carToUpdate.UpdateWithV2(requestData);
        await _carService.Update(carToUpdate);
        
        return NoContent();
    }

    /// <summary>
    /// Deletes the entry at given ID
    /// </summary>
    /// <param name="carId"></param>
    /// <response code="204">Successfully deleted entry at given ID</response>
    /// <response code="404">Car with given ID does not exist</response>
    [HttpDelete("{carId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int carId)
    {
        var car = await _carService.GetById(carId);

        if (car is null)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }

        await _carService.Delete(car);
        return NoContent();
    }
}