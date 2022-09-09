namespace JunkyardWebApp.API.Models;

public class Car
{
    public int CarId { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public List<Part>? AvailableParts { get; set; }
}