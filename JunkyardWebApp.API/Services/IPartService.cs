using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Services;

public interface IPartService
{
    Task<bool> CarExists(int carId);
    Task<bool> PartExistsForCar(int carId, int partId);
    Task<bool> PartExistsInDb(int partId);
    Task<ICollection<Part>> GetAllParts();
    Task<ICollection<Part>> GetPartsByCarId(int carId);
    Task<Part?> GetPartById(int partId);
    Task Update(Part part);
    Task Add(Part part);
    Task Delete(Part part);
}