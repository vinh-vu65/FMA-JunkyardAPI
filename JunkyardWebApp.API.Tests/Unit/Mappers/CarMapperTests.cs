using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Tests.Unit.Mappers;

public class CarMapperTests
{
    private readonly Car _car = new() {CarId = 1, Year = 2020, Make = "Testla", Model = "X", AvailableParts = null};
    private readonly CarWriteDto _testRequest = new() { Year = 2022, Make = "Toyota", Model = "Camry"};
    
    [Fact]
    public void ToDto_ShouldReturnNewCarReadDto_WhenGivenCarObject()
    {
        var expected = new CarReadDto { CarId = 1, Year = 2020, Make = "Testla", Model = "X", AvailablePartsCount = 0 };

        var result = _car.ToDto();
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpdateWith_ShouldUpdateGivenCarObjectWithGivenDtoProperties_WhenGivenCarAndDto()
    {
        var expected = _car with {Year = 2022, Make = "Toyota", Model = "Camry"};

        _car.UpdateWith(_testRequest);
        
        Assert.Equal(expected, _car);
    }
}