using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;

namespace JunkyardWebApp.API.Services;

public class CarService : ICarService
{
    private readonly IRepository<Car> _carRepository;

    public CarService(IRepository<Car> carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<bool> CarExistsInDb(int carId)
    {
        var car = await GetById(carId);
        return car is not null;
    }

    public async Task<ICollection<Car>> GetAll()
    {
        return await _carRepository.Get();
    }

    public async Task<Car?> GetById(int id)
    {
        return await _carRepository.GetById(id);
    }

    public async Task Update(Car car)
    {
        await _carRepository.Update(car);
    }

    public async Task Add(Car car)
    {
        await _carRepository.Add(car);
    }

    public async Task Delete(Car car)
    {
        await _carRepository.Delete(car);
    }
}