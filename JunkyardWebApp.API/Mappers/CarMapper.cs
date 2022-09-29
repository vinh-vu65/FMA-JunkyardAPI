using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Mappers;

public static class CarMapper
{ 
    public static void UpdateWith(this Car car, CarWriteDto dto)
    {
        car.Year = dto.Year;
        car.Make = dto.Make;
        car.Model = dto.Model;
    }

    public static CarReadDto ToDto(this Car car)
    {
        return new CarReadDto
        {
            CarId = car.CarId,
            Year = car.Year,
            Make = car.Make,
            Model = car.Model,
            AvailablePartsCount = car.AvailableParts?.Count ?? 0 
        };
    }
}