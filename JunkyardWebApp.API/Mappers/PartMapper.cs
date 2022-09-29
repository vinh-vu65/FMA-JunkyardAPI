using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Mappers;

public static class PartMapper
{
    public static PartReadDto ToDto(this Part part)
    {
        return new PartReadDto
        {
            CarId = part.CarId,
            PartId = part.PartId,
            Year = part.Car!.Year,
            Make = part.Car.Make,
            Model = part.Car.Model,
            Category = part.Category,
            Description = part.Description,
            Price = part.Price
        };
    }

    public static void UpdateWith(this Part part, PartWriteDto dto)
    {
        part.Category = dto.Category;
        part.Description = dto.Description;
        part.Price = dto.Price;
    }
}