using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;
using JunkyardWebApp.API.Services;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Unit.Services;

[Trait("Category", "Unit")]
public class PartServiceTests
{
    private readonly IPartRepository _partRepository = Substitute.For<IPartRepository>();
    private readonly IRepository<Car> _carRepository = Substitute.For<IRepository<Car>>();
    private readonly Car _car = new();
    private readonly Part _part = new();
    private readonly List<Part> _partsList = new();
    private readonly PartService _sut;

    public PartServiceTests()
    {
        _sut = new PartService(_partRepository, _carRepository);
    }
    
    [Fact]
    public async Task CarExists_ShouldReturnTrue_WhenGivenCarIdExistsInCarRepository()
    {
        _carRepository.GetById(Arg.Any<int>()).Returns(_car);

        var result = await _sut.CarExists(1);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task CarExists_ShouldReturnFalse_WhenGivenCarIdIsNullInRepository()
    {
        _carRepository.GetById(Arg.Any<int>()).Returns((Car?)null);

        var result = await _sut.CarExists(1);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task PartExistsForCar_ShouldReturnFalse_WhenGivenPartIdDoesNotExistInCarsParts()
    {
        _partRepository.GetPartsByCarId(Arg.Any<int>()).Returns(_partsList);

        var result = await _sut.PartExistsForCar(1, 1);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task PartExistsForCar_ShouldReturnTrue_WhenGivenPartIdDoesExistInCarsParts()
    {
        var partId = 1;
        _part.PartId = partId;
        _partsList.Add(_part);
        _partRepository.GetPartsByCarId(Arg.Any<int>()).Returns(_partsList);

        var result = await _sut.PartExistsForCar(1, 1);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task PartExistsInDb_ShouldReturnFalse_WhenGivenPartIdDoesNotExistInPartsRepository()
    {
        _partRepository.Get().Returns(_partsList);

        var result = await _sut.PartExistsInDb(1);
        
        Assert.False(result);
    }
    
    [Fact]
    public async Task PartExistsInDb_ShouldReturnTrue_WhenGivenPartIdDoesExistInPartsRepository()
    {
        var partId = 1;
        _part.PartId = partId;
        _partsList.Add(_part);
        _partRepository.Get().Returns(_partsList);

        var result = await _sut.PartExistsInDb(1);
        
        Assert.True(result);
    }
}