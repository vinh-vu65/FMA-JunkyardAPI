using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Tests.Unit.Mappers;

public class PartMapperTests
{
    private readonly Car _car = new() {CarId = 1, Year = 2020, Make = "Testla", Model = "X", AvailableParts = null};
    private readonly Part _part = new() { CarId = 1, PartId = 1, Category = PartsCategory.Engine, Description = "Test", Price = 1M };
    private readonly PartWriteDto _testRequest = new() { Category = "Brakes", Description = "Hello", Price = 35M};

    public PartMapperTests()
    {
        _part.Car = _car;
    }
    
    [Fact]
    public void ToDto_ShouldReturnNewPartReadDto_WhenGivenPartObject()
    {
        var expected = new PartReadDto
        {
            CarId = 1,
            PartId = 1,
            Year = 2020,
            Make = "Testla",
            Model = "X",
            Category = PartsCategory.Engine,
            Description = "Test",
            Price = 1M
        };

        var result = _part.ToDto();
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void UpdateWith_ShouldUpdateGivenPartObjectWithGivenDtoProperties_WhenGivenPartAndDto()
    {
        var expected = _part with {Category = PartsCategory.Brakes, Description = "Hello", Price = 35M};

        _part.UpdateWith(_testRequest);
        
        Assert.Equal(expected, _part);
    }

    [Theory]
    [InlineData("engine", PartsCategory.Engine)]
    [InlineData("eNgIne", PartsCategory.Engine)]
    [InlineData("Brakes", PartsCategory.Brakes)]
    public void UpdateWith_ShouldAcceptCategoryStringRegardlessOfLetterCase(string inputString, PartsCategory expected)
    {
        var testRequest = new PartWriteDto
        {
            Category = inputString,
            Description = "throwaway",
            Price = 56M
        };
        
        _part.UpdateWith(testRequest);
        
        Assert.Equal(expected, _part.Category);
    }
}