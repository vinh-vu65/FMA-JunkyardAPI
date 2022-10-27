using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace JunkyardWebApp.API.SwaggerExamples.Responses;

public class PartV1ResponseExample : IExamplesProvider<PartReadDtoV1>
{
    public PartReadDtoV1 GetExamples()
    {
        return new PartReadDtoV1
        {
            CarId = 2,
            PartId = 3,
            Year = 2003,
            Make = "Subaru",
            Model = "Impreza",
            Category = PartsCategory.Light,
            Description = "Light for Impreza",
            Price = 200.00M
        };
    }
}

public class PartV2ResponseExample : IExamplesProvider<PartReadDtoV2>
{
    public PartReadDtoV2 GetExamples()
    {
        return new PartReadDtoV2
        {
            CarId = 2,
            PartId = 3,
            Year = 2003,
            Make = "Subaru",
            Model = "Impreza",
            Colour = CarColour.Blue,
            Category = PartsCategory.Light,
            Description = "Light for Impreza",
            Price = 200.00M
        };
    }
}