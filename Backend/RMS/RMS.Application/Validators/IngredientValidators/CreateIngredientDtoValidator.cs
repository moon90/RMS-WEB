using FluentValidation;
using RMS.Application.DTOs.IngredientDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.IngredientValidators
{
    public class CreateIngredientDtoValidator : AbstractValidator<CreateIngredientDto>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public CreateIngredientDtoValidator(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ingredient Name is required.")
                .MaximumLength(100).WithMessage("Ingredient Name cannot exceed 100 characters.")
                .MustAsync(BeUniqueIngredientName).WithMessage("An ingredient with this name already exists.");

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

        private async Task<bool> BeUniqueIngredientName(string name, CancellationToken cancellationToken)
        {
            return await _ingredientRepository.GetQueryable().AllAsync(i => i.Name != name, cancellationToken);
        }
    }
}
