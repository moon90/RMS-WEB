using FluentValidation;
using RMS.Application.DTOs.Promotions;

namespace RMS.Application.Validators
{
    public class CreatePromotionDtoValidator : AbstractValidator<CreatePromotionDto>
    {
        public CreatePromotionDtoValidator()
        {
            RuleFor(x => x.CouponCode)
                .NotEmpty().WithMessage("Coupon Code is required.")
                .Length(3, 50).WithMessage("Coupon Code must be between 3 and 50 characters.");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount Amount cannot be negative.");

            RuleFor(x => x.DiscountPercentage)
                .InclusiveBetween(0, 100).WithMessage("Discount Percentage must be between 0 and 100.");

            RuleFor(x => x.ValidFrom)
                .NotEmpty().WithMessage("Valid From date is required.");

            RuleFor(x => x.ValidTo)
                .NotEmpty().WithMessage("Valid To date is required.")
                .GreaterThanOrEqualTo(x => x.ValidFrom).WithMessage("Valid To date must be after or equal to Valid From date.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");
        }
    }
}