using JunkyardWebApp.API.Controllers;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Enums;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Unit.Controllers;

[Trait("Category", "Unit")]
public class PartsControllerTests
{
    private readonly IPartService _partService = Substitute.For<IPartService>();
    private readonly Part[] _parts;
    private readonly Car _car = new() { CarId = 1, Make = "Test", Model = "123", Year = 2020, AvailableParts = null };
    private readonly Part _part = new() 
        { PartId = 1, Category = PartsCategory.Engine, Description = "Test part", Price = 10M, CarId = 1 };
    private readonly Part _part2 = new ()
        { PartId = 2, Category = PartsCategory.Brakes, Description = "Test brakes", Price = 20M, CarId = 1 };
    private readonly PartWriteDto _testRequest = new() 
        { Category = "Door", Description = "Test part request", Price = 23.0M };

    public PartsControllerTests()
    {
        _part.Car = _car;
        _part2.Car = _car;
        _parts = new[] {_part, _part2};
    }

    [Fact]
    public async Task GetAll_ShouldReturn200StatusCodeAndAllPartsDto()
    {
        _partService.GetAll().Returns(_parts);
        var controller = new PartsController(_partService);
        var firstPartDto = new PartReadDto()
        {
            CarId = 1,
            PartId = 1,
            Category = PartsCategory.Engine,
            Year = 2020,
            Make = "Test",
            Model = "123",
            Description = "Test part",
            Price = 10M
        };
        var secondPartDto = new PartReadDto()
        {
            CarId = 1,
            PartId = 2,
            Category = PartsCategory.Brakes,
            Year = 2020,
            Make = "Test",
            Model = "123",
            Description = "Test brakes",
            Price = 20M
        };
        var expected = new[] {firstPartDto, secondPartDto};

        var result = await controller.GetAll() as ObjectResult;
        
        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task GetPartsByCarId_ShouldReturn404StatusCode_WhenCarIdIsInvalid()
    {
        var carId = 45;
        _partService.CarExists(carId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.GetPartsByCarId(carId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task GetPartsByCarId_ShouldReturn200StatusCodeAndPartDto_WhenGivenValidId()
    {
        var carId = 23;
        _partService.GetPartsByCarId(23).Returns(_parts);
        _partService.CarExists(carId).Returns(true);
        var controller = new PartsController(_partService);
        var firstPartDto = new PartReadDto()
        {
            CarId = 1,
            PartId = 1,
            Category = PartsCategory.Engine,
            Year = 2020,
            Make = "Test",
            Model = "123",
            Description = "Test part",
            Price = 10M
        };
        var secondPartDto = new PartReadDto()
        {
            CarId = 1,
            PartId = 2,
            Category = PartsCategory.Brakes,
            Year = 2020,
            Make = "Test",
            Model = "123",
            Description = "Test brakes",
            Price = 20M
        };
        var expected = new[] {firstPartDto, secondPartDto};

        var result = await controller.GetPartsByCarId(carId) as ObjectResult;
        
        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task GetById_ShouldReturn404StatusCode_WhenCarIdIsInvalid()
    {
        var carId = 45;
        var partId = 34;
        _partService.CarExists(carId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.GetById(carId, partId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task GetById_ShouldReturn404StatusCode_WhenCarIdIsValidButPartIdIsInvalid()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.GetById(carId, partId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task GetById_ShouldReturn200StatusCodeWithPartDto_WhenCarIdAndPartIdIsValid()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(true);
        _partService.GetById(partId).Returns(_part);
        var controller = new PartsController(_partService);
        var expected = new PartReadDto()
        {
            CarId = 1,
            PartId = 1,
            Category = PartsCategory.Engine,
            Year = 2020,
            Make = "Test",
            Model = "123",
            Description = "Test part",
            Price = 10M
        };

        var result = await controller.GetById(carId, partId) as ObjectResult;

        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task Add_ShouldReturn404StatusCode_WhenCarIdIsInvalid()
    {
        var carId = 45;
        _partService.CarExists(carId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.Add(carId, _testRequest) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task Add_ShouldReturn400StatusCode_WhenCarIdIsValidAndGivenPartIdAlreadyExists()
    {
        var carId = 45;
        var partId = 23;
        _partService.PartExistsInDb(partId).Returns(true);
        _partService.CarExists(carId).Returns(true);
        var controller = new PartsController(_partService);

        var result = await controller.Add(carId, _testRequest, partId) as ObjectResult;

        Assert.Equal(400, result!.StatusCode);
    }
    
    [Fact]
    public async Task Add_ShouldReturn201StatusCodeAndCallAddOnCarService_WhenCarIdAndPartIdAreValid()
    {
        var carId = 45;
        var partId = 23;
        _partService.PartExistsInDb(partId).Returns(false);
        _partService.CarExists(carId).Returns(true);
        var controller = new PartsController(_partService);
        var expectedPart = new Part
        {
            PartId = partId,
            CarId = carId,
            Category = PartsCategory.Door,
            Description = _testRequest.Description,
            Price = _testRequest.Price
        };

        var result = await controller.Add(carId, _testRequest, partId) as ObjectResult;

        Assert.Equal(201, result!.StatusCode);
        await _partService.Received(1).Add(expectedPart);
    }
    
    [Fact]
    public async Task Update_ShouldReturn404StatusCode_WhenCarIdIsInvalid()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.Update(carId, _testRequest, partId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task Update_ShouldReturn201StatusCodeAndCallAddOnCarService_WhenGivenPartIdDoesNotYetExist()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(false);
        _partService.PartExistsInDb(partId).Returns(false);
        var controller = new PartsController(_partService);
        var expectedPart = new Part
        {
            PartId = partId,
            CarId = carId,
            Category = PartsCategory.Door,
            Description = _testRequest.Description,
            Price = _testRequest.Price
        };

        var result = await controller.Update(carId, _testRequest, partId) as ObjectResult;

        Assert.Equal(201, result!.StatusCode);
        await _partService.Received(1).Add(expectedPart);
    }
    
    [Fact]
    public async Task Update_ShouldReturn400StatusCode_WhenGivenPartIdMatchesDifferentPartInDb()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(false);
        _partService.PartExistsInDb(partId).Returns(true);
        var controller = new PartsController(_partService);

        var result = await controller.Update(carId, _testRequest, partId) as ObjectResult;

        Assert.Equal(400, result!.StatusCode);
    }
    
    [Fact]
    public async Task Update_ShouldReturn204StatusCodeAndCallUpdateOnService_WhenGivenValidCarIdAndPartId()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(true);
        _partService.PartExistsInDb(partId).Returns(true);
        _partService.GetById(partId).Returns(_part);
        var controller = new PartsController(_partService);
        var expectedPart = new Part()
        {
            PartId = _part.PartId,
            CarId = _part.CarId,
            Category = PartsCategory.Door,
            Description = _testRequest.Description,
            Price = _testRequest.Price,
            Car = _part.Car
        };

        var result = await controller.Update(carId, _testRequest, partId) as StatusCodeResult;

        Assert.Equal(204, result!.StatusCode);
        await _partService.Received(1).Update(expectedPart);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn404StatusCode_WhenCarIdIsInvalid()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.Delete(carId, partId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn404StatusCode_WhenPartIdDoesNotExistInGivenCarId()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(false);
        var controller = new PartsController(_partService);

        var result = await controller.Delete(carId, partId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn204StatusCodeAndCallDeleteOnService_WhenGivenValidPartAndCarId()
    {
        var carId = 45;
        var partId = 23;
        _partService.CarExists(carId).Returns(true);
        _partService.PartExistsForCar(carId, partId).Returns(true);
        _partService.GetById(partId).Returns(_part);
        var controller = new PartsController(_partService);

        var result = await controller.Delete(carId, partId) as StatusCodeResult;

        Assert.Equal(204, result!.StatusCode);
        await _partService.Received(1).Delete(_part);
    }
}