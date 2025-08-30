using FluentValidation;
using RMS.Application.DTOs.CategoryDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryCreateDtoValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.")
                .MustAsync(BeUniqueCategoryName).WithMessage("Category name already exists.");
        }

        private async Task<bool> BeUniqueCategoryName(string categoryName, CancellationToken cancellationToken)
        {
            return await _categoryRepository.GetCategoryByNameAsync(categoryName) == null;
        }
    }
}
