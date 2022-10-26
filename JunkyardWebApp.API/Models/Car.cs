using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Models;

public record Car
{
    public int CarId { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public CarColour Colour { get; set; }
    public ICollection<Part>? AvailableParts { get; set; }
}