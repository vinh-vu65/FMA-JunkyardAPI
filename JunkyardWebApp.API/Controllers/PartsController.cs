using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JunkyardWebApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PartsController : ControllerBase
{
    private readonly IRepository<Part> _partRepository;

    public PartsController(IRepository<Part> partRepository)
    {
        _partRepository = partRepository;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _partRepository.Get();
        return Ok(parts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var part = await _partRepository.GetById(id);
        if (part is null)
        {
            return NotFound();
        }
        return Ok(part);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody]Part part)
    {
        await _partRepository.Add(part);

        return CreatedAtAction(
            "GetById",
            new {id = part.PartId},
            part);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody]Part part, int id)
    {
        if (id != part.PartId)
        {
            return BadRequest();
        }
        
        if (await _partRepository.GetById(id) is null)
        {
            return NotFound();
        }
        
        await _partRepository.Update(part);
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var part = await _partRepository.GetById(id);

        if (part is null)
        {
            return NotFound();
        }

        await _partRepository.Delete(part);

        return NoContent();
    }
}