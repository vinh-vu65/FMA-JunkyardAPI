using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Dtos;

public record PostPutPartDto
{
    public PartsCategory Category { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}