using JunkyardWebApp.API.Controllers;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Unit.Controller;

public class CarsControllerTests
{
    private readonly ICarService _carService = Substitute.For<ICarService>();
    private readonly Car _car = new() {CarId = 1, Year = 1999, Make = "Test", Model = "Car"};
    private readonly Car _car2 = new() {CarId = 2, Year = 2020, Make = "Testla", Model = "X"};
    private readonly Car[] _carsInDb;
    
    public CarsControllerTests()
    {
        _carsInDb = new[] {_car, _car2};
    }
    
    [Fact]
    public async Task GetByID_ShouldReturnGetCarDtoAnd200StatusCode_WhenGivenValidId()
    {
        _carService.GetById(Arg.Any<int>()).Returns(_car);
        var controller = new CarsController(_carService);
        var expected = new CarReadDto
        {
            CarId = 1,
            Year = 1999,
            Make = "Test",
            Model = "Car",
            AvailablePartsCount = 0
        };

        var result = await controller.GetById(1) as ObjectResult;

        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task GetByID_ShouldReturn404StatusCode_WhenGivenInvalidId()
    {
        _carService.GetById(Arg.Any<int>()).Returns((Car?) null);
        var controller = new CarsController(_carService);

        var result = await controller.GetById(1) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200StatusCodeAndAllCarsDto()
    {
        _carService.GetAll().Returns(_carsInDb);
        var controller = new CarsController(_carService);
        var firstCarDto = new CarReadDto
        {
            CarId = 1,
            Year = 1999,
            Make = "Test",
            Model = "Car",
            AvailablePartsCount = 0
        };
        var secondCarDto = new CarReadDto
        {
            CarId = 2,
            Year = 2020,
            Make = "Testla",
            Model = "X",
            AvailablePartsCount = 0
        };
        var expected = new[] {firstCarDto, secondCarDto};

        var result = await controller.GetAll() as ObjectResult;

        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(expected, result.Value);
    }
    
    [Fact]
    public async Task Add_ShouldReturn201StatusCode_WhenGivenValidRequestDto()
    {
        var request = new CarWriteDto
        {
            Make = "Test",
            Model = "Dto",
            Year = 2022
        };
        var controller = new CarsController(_carService);

        var result = await controller.Add(request) as ObjectResult;

        Assert.Equal(201, result!.StatusCode);
    }
    
    [Fact]
    public async Task Add_Return400StatusCode_WhenGivenCarIdOfAnotherCarInDb()
    {
        var request = new CarWriteDto
        {
            Make = "Test",
            Model = "Dto",
            Year = 2022
        };
        var carId = 34;
        _carService.CarExistsInDb(carId).Returns(true);
        var controller = new CarsController(_carService);

        var result = await controller.Add(request, carId) as ObjectResult;

        Assert.Equal(400, result!.StatusCode);
    }
    
    [Fact]
    public async Task Add_ShouldCallAddOnCarServiceWithGivenCarId_WhenGivenCarId()
    {
        var request = new CarWriteDto
        {
            Make = "Test",
            Model = "Dto",
            Year = 2022
        };
        var carId = 45;
        var expectedCarToAdd = new Car {CarId = carId, Make = "Test", Model = "Dto", Year = 2022, AvailableParts = null};
        var controller = new CarsController(_carService);

        await controller.Add(request, carId);

        await _carService.Received(1).Add(expectedCarToAdd);
    }
    
    [Fact]
    public async Task Update_ShouldReturn204StatusCodeAndCallCarServiceUpdate()
    {
        var request = new CarWriteDto
        {
            Make = "Test",
            Model = "Dto",
            Year = 2022
        };
        var carId = 45;
        _carService.GetById(carId).Returns(new Car {CarId = carId});
        var expectedCarToUpdate = new Car {CarId = carId, Make = "Test", Model = "Dto", Year = 2022, AvailableParts = null};
        var controller = new CarsController(_carService);

        var result = await controller.Update(request, carId) as StatusCodeResult;

        await _carService.Received(1).Update(expectedCarToUpdate);
        Assert.Equal(204, result!.StatusCode);
    }
    
    [Fact]
    public async Task Update_ShouldCreateNewEntry_WhenGivenIdDoesNotExist()
    {
        var request = new CarWriteDto
        {
            Make = "Test",
            Model = "Dto",
            Year = 2022
        };
        var carId = 45;
        _carService.GetById(carId).Returns((Car?) null);
        var expectedCarToAdd = new Car {CarId = carId, Make = "Test", Model = "Dto", Year = 2022, AvailableParts = null};
        var controller = new CarsController(_carService);

        var result = await controller.Update(request, carId) as ObjectResult;

        await _carService.Received(1).Add(expectedCarToAdd);
        Assert.Equal(201, result!.StatusCode);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn404_WhenGivenIdDoesNotExist()
    {
        var carId = 45;
        _carService.GetById(carId).Returns((Car?) null);
        var controller = new CarsController(_carService);

        var result = await controller.Delete(carId) as ObjectResult;

        Assert.Equal(404, result!.StatusCode);
    }
    
    [Fact]
    public async Task Delete_ShouldReturn204_WhenGivenValidId()
    {
        var carId = 45;
        _carService.GetById(carId).Returns(_car);
        var controller = new CarsController(_carService);

        var result = await controller.Delete(carId) as StatusCodeResult;

        await _carService.Received(1).Delete(_car);
        Assert.Equal(204, result!.StatusCode);
    }
}