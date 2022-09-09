using System.Text.Json.Serialization;

namespace JunkyardWebApp.API.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal OrderTotal { get; set; }
    public ICollection<Part> OrderItems { get; set; }
    [JsonIgnore] 
    public Customer Customer { get; set; }
}