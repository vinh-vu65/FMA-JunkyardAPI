using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using JunkyardWebApp.API.Configurations;
using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;
using JunkyardWebApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
var root = Directory.GetCurrentDirectory();
var localEnvironmentFilePath = Path.Combine(root, ".env.local");
if (File.Exists(localEnvironmentFilePath))
{
    DotEnv.Load(localEnvironmentFilePath);
}

// Add health check
builder.Services.AddHealthChecks();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Repositories
builder.Services.AddScoped<IRepository<Car>, CarRepository>();
builder.Services.AddScoped<IPartRepository, PartRepository>();

// Business Logic/Service Layer
builder.Services.AddScoped<IPartService, PartService>();
builder.Services.AddScoped<ICarService, CarService>();

// Validators
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Db Context
builder.Services.AddScoped<IDbSeeder, DbSeeder>();
builder.Services.AddDbContext<JunkyardContext>(options => options.UseNpgsql(DbConfiguration.GetDbConnectionString()));

// Add API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = ApiVersion.Default;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

// Add Swagger annotations and example requests
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    options.ExampleFilters();
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

var app = builder.Build();

app.MapHealthChecks("/health");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.ApiVersion.ToString()
            );
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsProduction())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
  
        var context = services.GetRequiredService<JunkyardContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}

app.Run();