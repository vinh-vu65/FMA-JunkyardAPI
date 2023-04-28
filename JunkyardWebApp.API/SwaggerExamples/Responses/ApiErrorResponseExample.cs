using JunkyardWebApp.API.Models;
using Swashbuckle.AspNetCore.Filters;

namespace JunkyardWebApp.API.SwaggerExamples.Responses;

public class ApiErrorResponseExample : IExamplesProvider<ApiErrorResponse>
{
    public ApiErrorResponse GetExamples()
    {
        return new ApiErrorResponse
        {
            StatusCode = 404,
            Message = "Object not found"
        };
    }
}