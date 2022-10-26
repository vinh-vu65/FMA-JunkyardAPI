using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Mappers;

public static class CarMapper
{ 
    public static void UpdateWithV1(this Car car, CarWriteDtoV1 dto)
    {
        car.Year = dto.Year;
        car.Make = dto.Make;
        car.Model = dto.Model;
        car.Colour = CarColour.Default;
    }

    public static CarReadDtoV1 ToDtoV1(this Car car)
    {
        return new CarReadDtoV1
        {
            CarId = car.CarId,
            Year = car.Year,
            Make = car.Make,
            Model = car.Model,
            AvailablePartsCount = car.AvailableParts?.Count ?? 0 
        };
    }
    
    public static void UpdateWithV2(this Car car, CarWriteDtoV2 dto)
    {
        car.Year = dto.Year;
        car.Make = dto.Make;
        car.Model = dto.Model;
        car.Colour = Enum.Parse<CarColour>(dto.Colour, true);
    }

    public static CarReadDtoV2 ToDtoV2(this Car car)
    {
        return new CarReadDtoV2
        {
            CarId = car.CarId,
            Year = car.Year,
            Make = car.Make,
            Model = car.Model,
            Colour = car.Colour,
            AvailablePartsCount = car.AvailableParts?.Count ?? 0 
        };
    }
}