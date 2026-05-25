using FluentValidation;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.Interfaces;

namespace RMS.Application.Validators.StockTransactionValidators
{
    public class CreateStockTransactionDtoValidator : AbstractValidator<CreateStockTransactionDto>
    {
        public CreateStockTransactionDtoValidator()
        {
            RuleFor(x => x.TransactionType)
                .NotEmpty().WithMessage("Transaction Type is required.")
                .Length(2, 10).WithMessage("Transaction Type must be between 2 and 10 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction Date is required.");

            RuleFor(x => x.Remarks)
                .MaximumLength(250).WithMessage("Remarks cannot exceed 250 characters.");

            RuleFor(x => x.TransactionSource)
                .MaximumLength(50).WithMessage("Transaction Source cannot exceed 50 characters.");

            RuleFor(x => x.AdjustmentType)
                .MaximumLength(20).WithMessage("Adjustment Type cannot exceed 20 characters.")
                .Must(x => string.IsNullOrEmpty(x) || x == "Addition" || x == "Subtraction")
                .WithMessage("Adjustment Type must be 'Addition' or 'Subtraction'.");

            RuleFor(x => x.Reason)
                .MaximumLength(250).WithMessage("Reason cannot exceed 250 characters.");

            RuleFor(x => x)
                .Must(x => x.ProductID.HasValue || x.IngredientID.HasValue)
                .WithMessage("Either Product ID or Ingredient ID must be provided.");

            When(x => !string.IsNullOrEmpty(x.AdjustmentType),
                () =>
                {
                    RuleFor(x => x.Reason).NotEmpty().WithMessage("Reason is required for stock adjustments.");
                });
        }
    }
}
