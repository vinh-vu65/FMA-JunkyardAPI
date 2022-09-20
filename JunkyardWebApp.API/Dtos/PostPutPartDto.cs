using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Dtos;

public class PostPutPartDto
{
    public int CarId { get; set; }
    public PartsCategory Category { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}