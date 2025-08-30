using FluentValidation;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories; // Added
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RMS.Infrastructure.Interfaces; // Added

namespace RMS.Application.Validators
{
    public class UserCreateDtoValidator : AbstractValidator<UserCreateDto>
    {
        private readonly IUserRepository _userRepository; // Added

        public UserCreateDtoValidator(IUserRepository userRepository) // Modified constructor
        {
            _userRepository = userRepository; // Added

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.")
                .MustAsync(BeUniqueUserName).WithMessage("Username already exists."); // Added uniqueness check

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

            RuleFor(u => u.FullName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists."); // Added uniqueness check

            RuleFor(u => u.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

            //RuleFor(u => u.RoleID)
            //    .GreaterThan(0).WithMessage("Role ID must be greater than 0.");
        }

        private async Task<bool> BeUniqueUserName(string userName, CancellationToken cancellationToken)
        {
            return await _userRepository.GetQueryable().AllAsync(u => u.UserName != userName, cancellationToken);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.GetQueryable().AllAsync(u => u.Email != email, cancellationToken);
        }
    }
}
