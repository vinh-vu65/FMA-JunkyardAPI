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
public class PartsEndpointTests : IDisposable
{
    private readonly HttpClient _client;
    private readonly Car _car = new() {CarId = 1, Make = "Test", Model = "X", Year = 1995};
    private readonly Part _part = new()
        {PartId = 1, CarId = 1, Category = PartsCategory.Engine, Description = "Test Part", Price = 15M};
    private readonly JunkyardContext _dbContext;
    private readonly IDbSeeder _dbSeeder = Substitute.For<IDbSeeder>();

    public PartsEndpointTests()
    {
        _part.Car = _car;
        
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
    public async Task Get_ShouldReturnOkAndAllEntriesInDatabase()
    {
        _dbContext.Cars.Add(_car);
        _dbContext.Parts.Add(_part);
        await _dbContext.SaveChangesAsync();
        var expected = new List<PartReadDto> {_part.ToDto()};
        
        var response = await _client.GetAsync("api/Parts");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<List<PartReadDto>>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task GetByCarId_ShouldReturnOkAndAllPartsInCar()
    {
        _dbContext.Cars.Add(_car);
        _dbContext.Parts.Add(_part);
        await _dbContext.SaveChangesAsync();
        var expected = new List<PartReadDto> {_part.ToDto()};
        var carId = _car.CarId;
        
        var response = await _client.GetAsync($"api/cars/{carId}/Parts");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<List<PartReadDto>>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }
    
    [Fact]
    public async Task GetByPartId_ShouldReturnOkAndEntryMatchingGivenId()
    {
        _dbContext.Cars.Add(_car);
        _dbContext.Parts.Add(_part);
        await _dbContext.SaveChangesAsync();
        var expected = _part.ToDto();
        var partId = _part.PartId;
        var carId = _car.CarId;
        
        var response = await _client.GetAsync($"api/cars/{carId}/Parts/{partId}");
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<PartReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task GetByCarId_ShouldReturnNotFound_WhenGivenCarIdDoesNotExist()
    {
        var carId = 100;

        var response = await _client.GetAsync($"api/cars/{carId}/Parts");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task GetByPartId_ShouldReturnNotFound_WhenGivenPartIdDoesNotExist()
    {
        _dbContext.Cars.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partId = 100;

        var response = await _client.GetAsync($"api/cars/{carId}/Parts/{partId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnCreatedWithNewEntry()
    {
        _dbContext.Cars.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partToAdd = new PartWriteDto {Category = "Engine", Description = "New Part", Price = 100M};
        var requestBody = JsonContent.Create(partToAdd);
        var expected = new PartReadDto
        {
            CarId = _car.CarId, PartId = 1, Category = PartsCategory.Engine, Description = partToAdd.Description,
            Price = partToAdd.Price
        };

        var response = await _client.PostAsync($"api/cars/{carId}/Parts", requestBody);
        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<PartReadDto>(responseString);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, responseObject);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequestIfGivenPartIdAlreadyExists()
    {
        _dbContext.Cars.Add(_car);
        _dbContext.Add(_part);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var existingPartId = _part.PartId;
        var partToAdd = new PartWriteDto {Category = "Engine", Description = "New Part", Price = 100M};
        var requestBody = JsonContent.Create(partToAdd);
        
        var response = await _client.PostAsync($"api/cars/{carId}/Parts?partId={existingPartId}", requestBody);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldReturnNotFound_WhenGivenCarIdDoesNotExist()
    {
        var carId = 100;
        var partToAdd = new PartWriteDto {Category = "Engine", Description = "New Part", Price = 100M};
        var requestBody = JsonContent.Create(partToAdd);
        
        var response = await _client.PostAsync($"api/cars/{carId}/Parts", requestBody);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Put_ShouldUpdateDbEntryAndReturnNoContent_WhenSuccessful()
    {
        _dbContext.Add(_car);
        _dbContext.Add(_part);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partId = _part.PartId;
        var updatedPartDto = new PartWriteDto {Category = "Brakes", Description = "New Part", Price = 100M};
        var requestBody = JsonContent.Create(updatedPartDto);
        var expected = _part with
        {
            Category = PartsCategory.Brakes, Description = updatedPartDto.Description,
            Price = updatedPartDto.Price
        };

        var response = await _client.PutAsync($"api/Cars/{carId}/Parts/{partId}", requestBody);
        var updatedPart = await _dbContext.Parts.FindAsync(partId);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(expected, updatedPart);
    }

    [Fact]
    public async Task Put_ShouldReturnCreatedAndCreateNewEntry_WhenGivenPartIdDoesNotExist()
    {
        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partId = _part.PartId;
        var updatedPartDto = new PartWriteDto {Category = "Brakes", Description = "New Part", Price = 100M};
        var requestBody = JsonContent.Create(updatedPartDto);
        var expected = _part with
        {
            Category = PartsCategory.Brakes, Description = updatedPartDto.Description,
            Price = updatedPartDto.Price
        };

        var response = await _client.PutAsync($"api/Cars/{carId}/Parts/{partId}", requestBody);
        var addedPart = await _dbContext.Parts.FindAsync(partId);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(expected, addedPart);
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContentAndDeletePartFromDb_WhenSuccessful()
    {
        _dbContext.Add(_car);
        _dbContext.Add(_part);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partId = _part.PartId;

        var response = await _client.DeleteAsync($"api/Cars/{carId}/Parts/{partId}");
        var deletedCar = await _dbContext.Parts.FindAsync(partId);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Null(deletedCar);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenGivenPartIdDoesNotExist()
    {
        _dbContext.Add(_car);
        await _dbContext.SaveChangesAsync();
        var carId = _car.CarId;
        var partId = _part.PartId;

        var response = await _client.DeleteAsync($"api/Cars/{carId}/Parts/{partId}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}