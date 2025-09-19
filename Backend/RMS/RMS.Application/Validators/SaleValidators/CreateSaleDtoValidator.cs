
using FluentValidation;
using RMS.Application.DTOs;

namespace RMS.Application.Validators.SaleValidators
{
    public class CreateSaleDtoValidator : AbstractValidator<CreateSaleDto>
    {
        public CreateSaleDtoValidator()
        {
            RuleFor(x => x.SaleDate).NotEmpty();
            RuleFor(x => x.TotalAmount).GreaterThan(0);
            RuleFor(x => x.FinalAmount).GreaterThan(0);
            RuleFor(x => x.PaymentMethod).NotEmpty();
            RuleFor(x => x.CategoryId).GreaterThan(0);
            RuleFor(x => x.SaleDetails).NotEmpty();
        }
    }
}
