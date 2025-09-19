
using FluentValidation;
using RMS.Application.DTOs;

namespace RMS.Application.Validators.UnitConversionValidators
{
    public class CreateUnitConversionDtoValidator : AbstractValidator<CreateUnitConversionDto>
    {
        public CreateUnitConversionDtoValidator()
        {
            RuleFor(x => x.FromUnitID).GreaterThan(0).WithMessage("From Unit ID must be greater than 0.");
            RuleFor(x => x.ToUnitID).GreaterThan(0).WithMessage("To Unit ID must be greater than 0.");
            RuleFor(x => x.ConversionFactor).GreaterThan(0).WithMessage("Conversion Factor must be greater than 0.");
            RuleFor(x => x.FromUnitID).NotEqual(x => x.ToUnitID).WithMessage("From Unit and To Unit cannot be the same.");
        }
    }
}
