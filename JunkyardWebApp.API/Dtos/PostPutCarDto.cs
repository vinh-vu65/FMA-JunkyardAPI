namespace JunkyardWebApp.API.Dtos;

public record PostPutCarDto
{
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
}