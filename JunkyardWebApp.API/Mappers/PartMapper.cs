using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models;

namespace JunkyardWebApp.API.Mappers;

public static class PartMapper
{
    public static GetPartDto ToDto(this Part part)
    {
        return new GetPartDto
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

    public static void UpdateWith(this Part part, PostPutPartDto dto)
    {
        part.Category = dto.Category;
        part.Description = dto.Description;
        part.Price = dto.Price;
    }
}