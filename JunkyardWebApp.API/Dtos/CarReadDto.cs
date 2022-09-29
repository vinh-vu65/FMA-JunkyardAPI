namespace JunkyardWebApp.API.Dtos;

public record CarReadDto
{
    public int CarId { get; init; }
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public int AvailablePartsCount { get; init; }
}