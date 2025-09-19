using FluentValidation;
using RMS.Application.DTOs.ProductIngredientDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.ProductIngredientValidators
{
    public class UpdateProductIngredientDtoValidator : AbstractValidator<UpdateProductIngredientDto>
    {
        private readonly IProductIngredientRepository _productIngredientRepository;

        public UpdateProductIngredientDtoValidator(IProductIngredientRepository productIngredientRepository)
        {
            _productIngredientRepository = productIngredientRepository;

            RuleFor(x => x.ProductIngredientID)
                .NotEmpty().WithMessage("Product Ingredient ID is required.");

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

            RuleFor(x => new { x.ProductID, x.IngredientID, x.ProductIngredientID })
                .MustAsync(async (dto, compositeKey, cancellationToken) =>
                {
                    var existingProductIngredient = await _productIngredientRepository.GetQueryable()
                        .FirstOrDefaultAsync(pi => pi.ProductID == compositeKey.ProductID && 
                                                 pi.IngredientID == compositeKey.IngredientID && 
                                                 pi.ProductIngredientID != compositeKey.ProductIngredientID, 
                                                 cancellationToken);
                    return existingProductIngredient == null;
                }).WithMessage("This product already contains this ingredient.");
        }
    }
}
