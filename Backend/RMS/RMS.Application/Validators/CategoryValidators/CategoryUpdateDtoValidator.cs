using FluentValidation;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryUpdateDtoValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.CategoryID)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.")
                .MustAsync(async (dto, categoryName, cancellationToken) =>
                {
                    var existingCategory = await _categoryRepository.GetQueryable()
                        .Where(c => c.CategoryName == categoryName && c.CategoryID != dto.CategoryID)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingCategory == null;
                }).WithMessage("Category name already exists.");
        }
    }
}
