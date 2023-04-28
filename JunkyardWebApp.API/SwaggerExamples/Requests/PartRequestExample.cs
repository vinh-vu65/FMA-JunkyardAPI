using JunkyardWebApp.API.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace JunkyardWebApp.API.SwaggerExamples.Requests;

public class PartRequestExample : IExamplesProvider<PartWriteDto>
{
    public PartWriteDto GetExamples()
    {
        return new PartWriteDto
        {
            Category = "Engine",
            Description = "Engine for car",
            Price = 1250.00M
        };
    }
}