using FluentValidation;
using RMS.Domain.DTOs.MenuDTOs.InputDTOs;

namespace RMS.Application.Validators
{
    public class MenuUpdateDtoValidator : AbstractValidator<MenuUpdateDto>
    {
        public MenuUpdateDtoValidator()
        {
            RuleFor(m => m.MenuID)
                .GreaterThan(0).WithMessage("Menu ID must be greater than 0.");

            RuleFor(m => m.MenuName)
                .NotEmpty().WithMessage("Menu name is required.")
                .MaximumLength(50).WithMessage("Menu name cannot exceed 50 characters.");

            RuleFor(m => m.MenuPath)
                .NotEmpty().WithMessage("Menu path is required.")
                .MaximumLength(200).WithMessage("Menu path cannot exceed 200 characters.");

            RuleFor(m => m.MenuIcon)
                .MaximumLength(50).WithMessage("Menu icon cannot exceed 50 characters.");

            RuleFor(m => m.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative number.");
        }
    }
}
