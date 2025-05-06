using FluentValidation;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;

namespace RMS.Application.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(u => u.UserID)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.");

            RuleFor(u => u.FullName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(u => u.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(u => u.RoleID)
                .GreaterThan(0).WithMessage("Role ID must be greater than 0.");
        }
    }
}
