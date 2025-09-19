
using FluentValidation;
using RMS.Application.DTOs;

namespace RMS.Application.Validators.PurchaseValidators
{
    public class CreatePurchaseDtoValidator : AbstractValidator<CreatePurchaseDto>
    {
        public CreatePurchaseDtoValidator()
        {
            RuleFor(p => p.SupplierId)
                .GreaterThan(0).WithMessage("Supplier ID must be greater than 0.");

            RuleFor(p => p.TotalAmount)
                .GreaterThan(0).WithMessage("Total amount must be greater than 0.");

            RuleFor(p => p.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.")
                .MaximumLength(50).WithMessage("Payment method cannot exceed 50 characters.");

            RuleFor(p => p.PurchaseDetails)
                .NotEmpty().WithMessage("Purchase details cannot be empty.")
                .Must(details => details.All(d => d.ProductId > 0 && d.Quantity > 0 && d.UnitPrice > 0))
                .WithMessage("All purchase details must have valid Product ID, Quantity, and Unit Price.");
        }
    }
}
