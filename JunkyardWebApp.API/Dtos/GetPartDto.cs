using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Dtos;

public class GetPartDto
{
    public int CarId { get; set; }
    public int PartId { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public PartsCategory Category { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}