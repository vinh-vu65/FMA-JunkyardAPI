namespace JunkyardWebApp.API.Dtos;

public record CarWriteDtoV1
{
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
}

public record CarWriteDtoV2
{
    public int Year { get; init; }
    public string Make { get; init; }
    public string Model { get; init; }
    public string Colour { get; init; }
}