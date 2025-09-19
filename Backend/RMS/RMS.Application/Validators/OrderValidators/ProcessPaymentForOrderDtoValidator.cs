using FluentValidation;
using RMS.Application.DTOs.Orders;

namespace RMS.Application.Validators.OrderValidators
{
    public class ProcessPaymentForOrderDtoValidator : AbstractValidator<ProcessPaymentForOrderDto>
    {
        public ProcessPaymentForOrderDtoValidator()
        {
            RuleFor(x => x.OrderID).NotEmpty().WithMessage("Order ID is required.");
            RuleFor(x => x.AmountReceived).GreaterThanOrEqualTo(0).WithMessage("Amount received must be a positive value or zero.");
            RuleFor(x => x.PaymentMethod).NotEmpty().WithMessage("Payment method is required.").MaximumLength(50);
        }
    }
}