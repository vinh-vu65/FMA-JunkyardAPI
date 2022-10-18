using System.Net;
using System.Net.Http.Json;
using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Integration;

[Trait("Category", "Integration")]
public class CarsControllerTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly Car _car = new() {CarId = 1, Make = "Test", Model = "X", Year = 1995, AvailableParts = null};
    private readonly JunkyardContext _dbContext;
    private readonly IDbSeeder _dbSeeder = Substitute.For<IDbSeeder>();

    public CarsControllerTests()
    {
        var options = new DbContextOptionsBuilder<JunkyardContext>()
            .UseInMemoryDatabase("testDb").Options;
         _dbContext = new JunkyardContext(options, _dbSeeder);
        
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(
            builder => builder.ConfigureServices(services =>
            {
                services.Remove(services.SingleOrDefault(s => s.ServiceType == typeof(IDbSeeder))!);
                services.Remove(services.SingleOrDefault(s => s.ServiceType == typeof(JunkyardContext))!);
                services.AddSingleton(_ => _dbSeeder);
                services.AddSingleton(_ => _dbContext);
            }));
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _client = factory.CreateClient();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _client.Dispose();
    }

    [Fact]
    public async Task Get_ReturnsOkWithAllCarsInTheDatabase()
    {

        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var expected = new List<CarReadDto> {_car.ToDto()};
        
        var response = await _client.GetAsync("api/Cars");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<List<CarReadDto>>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task GetById_ReturnsOkWithCarMatchingId_WhenGivenId()
    {
        var carId = _car.CarId;
        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var expected = _car.ToDto();
        
        var response = await _client.GetAsync($"api/Cars/{carId}");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<CarReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenIdDoesNotExist()
    {
        var carId = 100;

        var response = await _client.GetAsync($"api/cars/{carId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedWithNewEntry()
    {
        var newCar = new CarWriteDto {Year = 2022, Make = "Test", Model = "Car"};
        var requestBody = JsonContent.Create(newCar);
        var expected = new CarReadDto {CarId = 1, Year = newCar.Year, Make = newCar.Make, Model = newCar.Model, AvailablePartsCount = 0};

        var response = await _client.PostAsync("api/cars", requestBody);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<CarReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task Post_ShouldCreateNewItemWithGivenId_WhenIdIsGiven()
    {
        var newCar = new CarWriteDto {Year = 2022, Make = "Test", Model = "Car"};
        var carId = 45;
        var requestBody = JsonContent.Create(newCar);
        var expected = new CarReadDto {CarId = carId, Year = newCar.Year, Make = newCar.Make, Model = newCar.Model, AvailablePartsCount = 0};

        var response = await _client.PostAsync($"api/cars?id={carId}", requestBody);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<CarReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task Put_ShouldReturnNoContentAndUpdateDatabaseEntry_WhenSuccessful()
    {
        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carRequest = new CarWriteDto {Year = 2022, Make = "Test", Model = "Car"};
        var carId = _car.CarId;
        var requestBody = JsonContent.Create(carRequest);

        var response = await _client.PutAsync($"api/cars/{carId}", requestBody);
        var updatedCar = await _dbContext.Cars.FindAsync(carId);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(carRequest.Year, updatedCar.Year);
        Assert.Equal(carRequest.Make, updatedCar.Make);
        Assert.Equal(carRequest.Model, updatedCar.Model);
    }
    
    [Fact]
    public async Task Put_ShouldReturnCreatedAndCreateEntry_WhenGivenIdDoesNotExist()
    {
        var updatedCar = new CarWriteDto {Year = 2022, Make = "Test", Model = "Car"};
        var carId = 45;
        var requestBody = JsonContent.Create(updatedCar);
        var expected = new CarReadDto {CarId = carId, Year = updatedCar.Year, Make = updatedCar.Make, Model = updatedCar.Model, AvailablePartsCount = 0};

        var response = await _client.PutAsync($"api/cars/{carId}", requestBody);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<CarReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
    {
        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;

        var response = await _client.DeleteAsync($"api/cars/{carId}");
        var deletedEntry = await _dbContext.Cars.FindAsync(carId);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(deletedEntry);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenGivenIdDoesNotExist()
    {
        var carId = 100;

        var response = await _client.DeleteAsync($"api/cars/{carId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}