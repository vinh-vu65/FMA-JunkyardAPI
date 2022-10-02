using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Services;

public interface ICarService : IService<Car>
{
    Task<bool> CarExistsInDb(int carId);
}