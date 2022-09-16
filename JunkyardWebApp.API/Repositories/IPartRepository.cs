using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Repositories;

public interface IPartRepository : IRepository<Part>
{
    Task<ICollection<Part>?> GetPartsByCarId(int carId);
}