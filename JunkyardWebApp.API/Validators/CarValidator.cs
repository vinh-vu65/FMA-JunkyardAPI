using FluentValidation;
using JunkyardWebApp.API.Dtos;

namespace JunkyardWebApp.API.Validators;

public class CarValidator : AbstractValidator<CarWriteDto>
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