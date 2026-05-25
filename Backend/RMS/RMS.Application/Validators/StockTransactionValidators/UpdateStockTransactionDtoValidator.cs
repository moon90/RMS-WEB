using FluentValidation;
using RMS.Application.DTOs.StockTransactionDTOs.InputDTOs;
using RMS.Application.Interfaces;

namespace RMS.Application.Validators.StockTransactionValidators
{
    public class UpdateStockTransactionDtoValidator : AbstractValidator<UpdateStockTransactionDto>
    {
        public UpdateStockTransactionDtoValidator()
        {
            RuleFor(x => x.TransactionID)
                .NotEmpty().WithMessage("Transaction ID is required.");

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
