using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Dtos;

public record CarReadDtoV1
{
    public int CarId { get; init; }
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public int AvailablePartsCount { get; init; }
}

public record CarReadDtoV2
{
    public int CarId { get; init; }
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public CarColour Colour { get; init; }
    public int AvailablePartsCount { get; init; }
}