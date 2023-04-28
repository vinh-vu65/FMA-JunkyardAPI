using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Dtos;

public record PartReadDtoV1
{
    public int CarId { get; init; }
    public int PartId { get; init; }
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public PartsCategory Category { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}

public record PartReadDtoV2
{
    public int CarId { get; init; }
    public int PartId { get; init; }
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public CarColour Colour { get; init; }
    public PartsCategory Category { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}