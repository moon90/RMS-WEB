using FluentValidation;
using RMS.Application.DTOs.Orders;

namespace RMS.Application.Validators.OrderValidators
{
    public class CreateOrderDetailDtoValidator : AbstractValidator<CreateOrderDetailDto>
    {
        public CreateOrderDetailDtoValidator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be at least 1.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a positive value.");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must be a positive value.");
        }
    }
}