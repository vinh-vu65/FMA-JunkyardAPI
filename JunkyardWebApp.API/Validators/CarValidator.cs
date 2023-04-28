using FluentValidation;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Validators;

public class CarValidator : AbstractValidator<CarWriteDtoV1>
{
    public CarValidator()
    {
        RuleFor(car => car.Year)
            .GreaterThan(1970)
            .WithMessage("Year must be greater than 1970")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage($"Year must be less than or equal to {DateTime.Now.Year}");

        RuleFor(car => car.Make)
            .NotEmpty()
            .WithMessage("Make cannot be empty");
        
        RuleFor(car => car.Model)
            .NotEmpty()
            .WithMessage("Model cannot be empty");
    }
}

public class CarValidatorV2 : AbstractValidator<CarWriteDtoV2>
{
    public CarValidatorV2()
    {
        RuleFor(car => car.Year)
            .GreaterThan(1970)
            .WithMessage("Year must be greater than 1970")
            .LessThanOrEqualTo(DateTime.Now.Year)
            .WithMessage($"Year must be less than or equal to {DateTime.Now.Year}");

        RuleFor(car => car.Make)
            .NotEmpty()
            .WithMessage("Make cannot be empty");
        
        RuleFor(car => car.Model)
            .NotEmpty()
            .WithMessage("Model cannot be empty");
        
        RuleFor(car => car.Colour)
            .IsEnumName(typeof(CarColour), false)
            .WithMessage("Invalid car colour");
    }
}