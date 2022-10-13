using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;
using JunkyardWebApp.API.Services;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Unit.Services;

[Trait("Category", "Unit")]
public class CarServiceTests
{
    private readonly IRepository<Car> _carRepository = Substitute.For<IRepository<Car>>();
    private readonly Car _car = new();
    private readonly CarService _sut;
    
    public CarServiceTests()
    {
        _sut = new CarService(_carRepository);
    }

    [Fact]
    public async Task CarExistsInDb_ShouldReturnTrue_WhenGivenCarIdExistsInCarRepository()
    {
        _carRepository.GetById(Arg.Any<int>()).Returns(_car);

        var result = await _sut.CarExistsInDb(1);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task CarExistsInDb_ShouldReturnFalse_WhenGivenCarIdIsNullInRepository()
    {
        _carRepository.GetById(Arg.Any<int>()).Returns((Car?)null);

        var result = await _sut.CarExistsInDb(1);
        
        Assert.False(result);
    }
}