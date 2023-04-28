namespace JunkyardWebApp.API.Dtos;

public record PartWriteDto
{
    public string Category { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}