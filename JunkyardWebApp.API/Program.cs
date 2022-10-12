using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using JunkyardWebApp.API.Data;
using JunkyardWebApp.API.Models;
using JunkyardWebApp.API.Repositories;
using JunkyardWebApp.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDbContext<JunkyardContext>(options => options.UseInMemoryDatabase("Junkyard"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();