using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;

namespace JunkyardWebApp.API.Services;

public class CarService : IService<Car>
{
    private readonly IRepository<Car> _carRepository;

    public CarService(IRepository<Car> carRepository)
    {
        _carRepository = carRepository;
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