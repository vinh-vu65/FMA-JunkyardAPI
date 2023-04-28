using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;

namespace JunkyardWebApp.API.Services;

public class PartService : IPartService
{
    private readonly IPartRepository _partRepository;
    private readonly IRepository<Car> _carRepository;

    public PartService(IPartRepository partRepository, IRepository<Car> carRepository)
    {
        _partRepository = partRepository;
        _carRepository = carRepository;
    }
    
    public async Task<bool> CarExists(int carId)
    {
        var car = await _carRepository.GetById(carId);
        return car is not null;
    }

    public async Task<bool> PartExistsForCar(int carId, int partId)
    {
        var carParts = await _partRepository.GetPartsByCarId(carId);
        return carParts.Any(p => p.PartId == partId);
    }

    public async Task<bool> PartExistsInDb(int partId)
    {
        var allParts = await _partRepository.Get();
        return allParts.Any(p => p.PartId == partId);
    }

    public async Task<ICollection<Part>> GetAll()
    {
        return await _partRepository.Get();
    }

    public async Task<ICollection<Part>> GetPartsByCarId(int carId)
    {
        return await _partRepository.GetPartsByCarId(carId);
    }

    public async Task<Part?> GetById(int partId)
    {
        return await _partRepository.GetById(partId);
    }

    public async Task Update(Part part)
    {
        await _partRepository.Update(part);
    }

    public async Task Add(Part part)
    {
        await _partRepository.Add(part);
    }

    public async Task Delete(Part part)
    {
        await _partRepository.Delete(part);
    }
}