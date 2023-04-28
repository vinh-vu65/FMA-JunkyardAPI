using System.Net;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace JunkyardWebApp.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("/api/cars/{carId}/[controller]")]
[Consumes("application/json")]
[Produces("application/json")]
public class PartsController : ControllerBase
{
    private readonly IPartService _partService;

    public PartsController(IPartService partService)
    {
        _partService = partService;
    }
    
    /// <summary>
    ///     Retrieves all the parts in the database
    /// </summary>
    /// <response code="200">Successfully retrieved all the parts</response>
    [HttpGet("/api/[controller]")]
    [ProducesResponseType(typeof(PartReadDtoV1), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _partService.GetAll();
        var partsDto = parts.Select(p => p.ToDtoV1());
        return Ok(partsDto);
    }

    /// <summary>
    ///     Retrieves all the parts for given car ID
    /// </summary>
    /// <response code="200">Successfully retrieved all the parts for given car</response>
    /// <response code="404">Car with given ID does not exist</response>
    [HttpGet]
    [ProducesResponseType(typeof(PartReadDtoV1), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetPartsByCarId(int carId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound(new ApiErrorResponse{ StatusCode = 404, Message = "Car not found" });
        }
        
        var parts = await _partService.GetPartsByCarId(carId);

        var partsDto = parts.Select(p => p.ToDtoV1());
        return Ok(partsDto);
    }

    /// <summary>
    ///     Retrieves a specific part
    /// </summary>
    /// <response code="200">Successfully retrieved the specified part</response>
    /// <response code="404">One of the given IDs does not exist</response>
    [HttpGet("{partId}")]
    [ProducesResponseType(typeof(PartReadDtoV1), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetById(int carId, int partId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        if (!partExistsForCar)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "PartId not found for this car" });
        }

        var part = await _partService.GetById(partId);
        
        var partDto = part!.ToDtoV1();
        return Ok(partDto);
    }

    /// <summary>
    ///     Creates a new part entry for given car
    /// </summary>
    /// <response code="201">Successfully created a new entry</response>
    /// <response code="400">If an ID was given as a query parameter, a part with this ID already exists</response>
    /// <response code="404">Car with given ID does not exist</response>
    [HttpPost]
    [ProducesResponseType(typeof(PartReadDtoV1), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Add(int carId, [FromBody]PartWriteDto requestData, int? partId = null)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }

        var part = new Part { CarId = carId };
        if (partId.HasValue)
        {
            if (await _partService.PartExistsInDb(partId.Value))
            {
                return BadRequest(new ApiErrorResponse { StatusCode = 400, Message = "Another part already has this partId" });
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

    /// <summary>
    ///     Updates an entry with given ID
    /// </summary>
    /// <response code="201">Given ID did not yet exist, successfully created new entry</response>
    /// <response code="204">Successfully updated entry with given ID</response>
    /// <response code="400">Given part ID already exists with another car</response>
    /// <response code="404">Car with given ID does not exist</response>
    [HttpPut("{partId}")]
    [ProducesResponseType(typeof(PartReadDtoV1), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Update(int carId, [FromBody]PartWriteDto requestData, int partId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        var partExistsInDb = await _partService.PartExistsInDb(partId);
        
        if (!partExistsForCar && !partExistsInDb)
        {
            return await Add(carId, requestData, partId);
        }

        if (!partExistsForCar && partExistsInDb)
        {
            return BadRequest(new ApiErrorResponse { StatusCode = 400, Message = "Another part already has this partId" });
        }

        var partToUpdate = await _partService.GetById(partId);
        partToUpdate!.UpdateWith(requestData);
        await _partService.Update(partToUpdate!);

        return NoContent();
    }

    /// <summary>
    /// Deletes the entry at given ID
    /// </summary>
    /// <response code="204">Successfully deleted entry at given ID</response>
    /// <response code="404">One of the given IDs does not exist</response>
    [HttpDelete("{partId}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), (int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Delete(int carId, int partId)
    {
        var carExists = await _partService.CarExists(carId);
        if (!carExists)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "Car not found" });
        }

        var partExistsForCar = await _partService.PartExistsForCar(carId, partId);
        if (!partExistsForCar)
        {
            return NotFound(new ApiErrorResponse { StatusCode = 404, Message = "PartId not found for this car" });
        }

        var part = await _partService.GetById(partId);
        await _partService.Delete(part!);
        
        return NoContent();
    }
}