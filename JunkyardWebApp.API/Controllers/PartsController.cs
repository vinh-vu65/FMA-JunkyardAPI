using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("/cars/{carId}/[controller]")]
public class PartsController : ControllerBase
{
    private readonly IPartService _partService;

    public PartsController(IPartService partService)
    {
        _partService = partService;
    }
    
    [HttpGet("/[controller]")]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _partService.GetAll();
        var partsDto = parts.Select(p => p.ToDto());
        return Ok(partsDto);
    }

    [HttpGet]
    public async Task<IActionResult> GetPartsByCarId(int carId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound("Car not found");
        }
        
        var parts = await _partService.GetPartsByCarId(carId);

        var partsDto = parts.Select(p => p.ToDto());
        return Ok(partsDto);
    }

    [HttpGet("{partId}")]
    public async Task<IActionResult> GetById(int partId, int carId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound("Car not found");
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        if (!partExistsForCar)
        {
            return NotFound("PartId not found for this car");
        }

        var part = await _partService.GetById(partId);
        
        var partDto = part!.ToDto();
        return Ok(partDto);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int carId, [FromBody]PostPutPartDto requestData, int? partId = null)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound("Car not found");
        }

        var part = new Part { CarId = carId };
        if (partId.HasValue)
        {
            if (await _partService.PartExistsInDb(partId.Value))
            {
                return BadRequest("Another part already has this partId");
            }
            part.PartId = partId.Value;
        }
        
        part.UpdateWith(requestData);
        await _partService.Add(part);

        return CreatedAtAction(
            "GetById",
            new {partId = part.PartId, carId = part.CarId},
            part);
    }

    [HttpPut("{partId}")]
    public async Task<IActionResult> Update([FromBody]PostPutPartDto requestData, int partId, int carId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound("Car was not found");
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        var partExistsInDb = await _partService.PartExistsInDb(partId);
        
        if (!partExistsForCar && !partExistsInDb)
        {
            return await Add(carId, requestData, partId);
        }

        if (partExistsInDb)
        {
            return BadRequest("Another part already has this partId");
        }

        var partToUpdate = await _partService.GetById(partId);
        partToUpdate!.UpdateWith(requestData);
        await _partService.Update(partToUpdate!);

        return NoContent();

    }

    [HttpDelete("{partId}")]
    public async Task<IActionResult> Delete(int carId, int partId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound("Car not found");
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        if (!partExistsForCar)
        {
            return NotFound("PartId not found for this car");
        }

        var part = await _partService.GetById(partId);
        await _partService.Delete(part!);
        
        return NoContent();
    }
}

// var part = new Part {PartId = partId, CarId = carId};
// part.UpdateWith(requestData);
// await _partService.Add(part);
//
// return CreatedAtAction(
//     "GetById",
//     new {partId = part.PartId, carId = part.CarId},
//     part);