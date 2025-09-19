using FluentValidation;
using RMS.Application.DTOs.RoleDTOs.InputDTOs;

namespace RMS.Application.Validators
{
    public class RoleUpdateDtoValidator : AbstractValidator<RoleUpdateDto>
    {
        public RoleUpdateDtoValidator()
        {
            RuleFor(r => r.RoleID)
                .GreaterThan(0).WithMessage("Role ID must be greater than 0.");

            RuleFor(r => r.RoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.");

            RuleFor(r => r.Description)
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");
        }
    }
}
