using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Data;

public interface IPartRepository : IRepository<Part>
{
    Task<ICollection<Part>>? GetPartsByCarId(int carId);
}