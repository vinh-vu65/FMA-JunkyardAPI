namespace JunkyardWebApp.API.Dtos;

public class GetCarDto
{
    public int CarId { get; set; }
    public int Year { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int AvailablePartsCount { get; set; }
}