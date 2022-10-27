using JunkyardWebApp.API.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace JunkyardWebApp.API.SwaggerExamples.Requests;

public class CarV1RequestExample : IExamplesProvider<CarWriteDtoV1>
{
    public CarWriteDtoV1 GetExamples()
    {
        return new CarWriteDtoV1
        {
            Make = "Toyota",
            Model = "Corolla",
            Year = 2005
        };
    }
}

public class CarV2RequestExample : IExamplesProvider<CarWriteDtoV2>
{
    public CarWriteDtoV2 GetExamples()
    {
        return new CarWriteDtoV2
        {
            Make = "Toyota",
            Model = "Corolla",
            Colour = "Black",
            Year = 2005
        };
    }
}