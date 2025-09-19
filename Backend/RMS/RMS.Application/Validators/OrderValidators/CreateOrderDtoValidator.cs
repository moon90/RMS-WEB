using FluentValidation;
using RMS.Application.DTOs.Orders;

namespace RMS.Application.Validators.OrderValidators
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.OrderDate).NotEmpty().WithMessage("Order Date is required.");
            RuleFor(x => x.OrderTime).NotEmpty().WithMessage("Order Time is required.").MaximumLength(15);
            RuleFor(x => x.TableName).NotEmpty().WithMessage("Table Name is required.").MaximumLength(50);
            RuleFor(x => x.WaiterName).MaximumLength(50);
            RuleFor(x => x.OrderStatus).NotEmpty().WithMessage("Order Status is required.").MaximumLength(20);
            RuleFor(x => x.OrderType).NotEmpty().WithMessage("Order Type is required.").MaximumLength(20);
            RuleFor(x => x.Total).GreaterThanOrEqualTo(0).WithMessage("Total must be a positive value or zero.");
            RuleFor(x => x.DiscountAmount).GreaterThanOrEqualTo(0).WithMessage("Discount Amount must be a positive value or zero.");
            RuleFor(x => x.DiscountPercentage).InclusiveBetween(0, 100).WithMessage("Discount Percentage must be between 0 and 100.");
            RuleFor(x => x.Received).GreaterThanOrEqualTo(0).WithMessage("Received amount must be a positive value or zero.");
            RuleFor(x => x.ChangeAmount).GreaterThanOrEqualTo(0).WithMessage("Change Amount must be a positive value or zero.");

            RuleForEach(x => x.OrderDetails).SetValidator(new CreateOrderDetailDtoValidator());
        }
    }
}