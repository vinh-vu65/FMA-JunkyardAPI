namespace JunkyardWebApp.API.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public List<Order>? OrderHistory { get; set; }
}