using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Services;

public interface IPartService : IService<Part>
{
    Task<bool> CarExists(int carId);
    Task<bool> PartExistsForCar(int carId, int partId);
    Task<bool> PartExistsInDb(int partId);
    Task<ICollection<Part>> GetPartsByCarId(int carId);
}