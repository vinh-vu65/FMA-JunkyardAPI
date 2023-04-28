using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace JunkyardWebApp.API.SwaggerExamples.Responses;

public class CarV1ResponseExample : IExamplesProvider<CarReadDtoV1>
{
    public CarReadDtoV1 GetExamples()
    {
        return new CarReadDtoV1
        {
            CarId = 1,
            Year = 2002,
            Make = "Honda",
            Model = "Civic",
            AvailablePartsCount = 3
        };
    }
}

public class CarV2ResponseExample : IExamplesProvider<CarReadDtoV2>
{
    public CarReadDtoV2 GetExamples()
    {
        return new CarReadDtoV2
        {
            CarId = 1,
            Year = 2002,
            Colour = CarColour.White,
            Make = "Honda",
            Model = "Civic",
            AvailablePartsCount = 3
        };
    }
}
