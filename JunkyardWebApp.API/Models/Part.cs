using System.Text.Json.Serialization;
using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Models;

public class Part
{
    public int PartId { get; set; }
    public PartsCategory Category { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int CarId { get; set; }
    [JsonIgnore]
    public Car? Car { get; set; }
}