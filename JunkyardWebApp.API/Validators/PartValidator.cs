using FluentValidation;
using JunkyardWebApp.API.Dtos;
using JunkyardWebApp.API.Models.Enums;

namespace JunkyardWebApp.API.Validators;

public class PartValidator : AbstractValidator<PartWriteDto>
{
    public PartValidator()
    {
        RuleFor(p => p.Category)
            .IsEnumName(typeof(PartsCategory), false);

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Description cannot be empty");
        
        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");
    }
}