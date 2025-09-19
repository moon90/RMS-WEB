using FluentValidation;
using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.IngredientValidators
{
    public class UpdateIngredientDtoValidator : AbstractValidator<UpdateIngredientDto>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public UpdateIngredientDtoValidator(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;

            RuleFor(x => x.IngredientID)
                .NotEmpty().WithMessage("Ingredient ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ingredient Name is required.")
                .MaximumLength(100).WithMessage("Ingredient Name cannot exceed 100 characters.")
                .MustAsync(async (dto, name, cancellationToken) =>
                {
                    var existingIngredient = await _ingredientRepository.GetQueryable()
                        .FirstOrDefaultAsync(i => i.Name == name && i.IngredientID != dto.IngredientID, cancellationToken);
                    return existingIngredient == null;
                }).WithMessage("An ingredient with this name already exists.");

            RuleFor(x => x.QuantityAvailable)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity Available cannot be negative.");

            RuleFor(x => x.UnitID)
                .NotEmpty().WithMessage("Unit ID is required.");

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Reorder Level cannot be negative.");

            RuleFor(x => x.ReorderQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Reorder Quantity cannot be negative.");

            RuleFor(x => x.Remarks)
                .MaximumLength(250).WithMessage("Remarks cannot exceed 250 characters.");
        }
    }
}
