using System.Net;
using System.Net.Http.Json;
using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Mappers;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;

namespace JunkyardWebApp.API.Tests.Integration;

[Trait("Category", "Integration")]
public class ApiIntegrationTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly JunkyardContext _dbContext;
    private readonly IDbSeeder _dbSeeder = Substitute.For<IDbSeeder>();

    public ApiIntegrationTests()
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
    public async Task Get_ReturnsAllCarsInDB_WhenCalledOnCarsEndpoint()
    {
        var car = new Car {CarId = 2, Make = "Test", Model = "X", Year = 1995, Colour = CarColour.Red};
        _dbContext.Add(car);
        await _dbContext.SaveChangesAsync();
        var expected = new List<CarReadDtoV1> {car.ToDtoV1()};
        
        var response = await _client.GetAsync("api/Cars");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<List<CarReadDtoV1>>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task Get_ReturnsAllPartsInCar_WhenCalledOnGetByCarIdEndpoint()
    {
        var car = new Car {CarId = 1, Make = "Test", Model = "X", Year = 1995, Colour = CarColour.Red};
        var part = new Part
            {PartId = 1, CarId = 1, Category = PartsCategory.Engine, Description = "Test Part", Price = 15M, Car = car};
        _dbContext.Cars.Add(car);
        _dbContext.Parts.Add(part);
        await _dbContext.SaveChangesAsync();
        var expected = new List<PartReadDtoV1> {part.ToDtoV1()};
        var carId = car.CarId;
        
        var response = await _client.GetAsync($"api/cars/{carId}/Parts");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<List<PartReadDtoV1>>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task Post_ShouldReturnCreatedWithNewEntry()
    {
        var newCar = new CarWriteDtoV1 {Year = 2022, Make = "Test", Model = "Car"};
        var requestBody = JsonContent.Create(newCar);
        var expected = new CarReadDtoV1 {CarId = 1, Year = newCar.Year, Make = newCar.Make, Model = newCar.Model, AvailablePartsCount = 0};

        var response = await _client.PostAsync("api/cars", requestBody);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<CarReadDtoV1>(responseString);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnNoContentAndDeletePartFromDb_WhenSuccessful()
    {
        var car = new Car {CarId = 1, Make = "Test", Model = "X", Year = 1995, Colour = CarColour.Red};
        var part = new Part
            {PartId = 1, CarId = 1, Category = PartsCategory.Engine, Description = "Test Part", Price = 15M, Car = car};
        _dbContext.Add(car);
        _dbContext.Add(part);
        await _dbContext.SaveChangesAsync();
        var carId = car.CarId;
        var partId = part.PartId;

        var response = await _client.DeleteAsync($"api/Cars/{carId}/Parts/{partId}");
        var deletedCar = await _dbContext.Parts.FindAsync(partId);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(deletedCar);
    }
}