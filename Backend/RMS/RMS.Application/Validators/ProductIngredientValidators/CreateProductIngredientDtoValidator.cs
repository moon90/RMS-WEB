using FluentValidation;
using RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.ProductIngredientValidators
{
    public class CreateProductIngredientDtoValidator : AbstractValidator<CreateProductIngredientDto>
    {
        private readonly IProductIngredientRepository _productIngredientRepository;

        public CreateProductIngredientDtoValidator(IProductIngredientRepository productIngredientRepository)
        {
            _productIngredientRepository = productIngredientRepository;

            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(x => x.IngredientID)
                .NotEmpty().WithMessage("Ingredient ID is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitID)
                .NotEmpty().WithMessage("Unit ID is required.");

            RuleFor(x => x.Remarks)
                .MaximumLength(250).WithMessage("Remarks cannot exceed 250 characters.");

            RuleFor(x => x)
                .MustAsync(BeUniqueProductIngredient).WithMessage("This product already contains this ingredient.");
        }

        private async Task<bool> BeUniqueProductIngredient(CreateProductIngredientDto productIngredient, CancellationToken cancellationToken)
        {
            return await _productIngredientRepository.GetQueryable().AllAsync(
                pi => pi.ProductID != productIngredient.ProductID || pi.IngredientID != productIngredient.IngredientID,
                cancellationToken
            );
        }
    }
}
