using FluentValidation;
using RMS.Domain.DTOs.UserDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories; // Added
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Added
using System.Linq;
using RMS.Infrastructure.Interfaces; // Added

namespace RMS.Application.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        private readonly IUserRepository _userRepository; // Added

        public UserUpdateDtoValidator(IUserRepository userRepository) // Modified constructor
        {
            _userRepository = userRepository; // Added

            RuleFor(u => u.UserID)
                .GreaterThan(0).WithMessage("User ID must be greater than 0.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(100).WithMessage("Username cannot exceed 100 characters.")
                .MustAsync(async (dto, userName, cancellationToken) =>
                {
                    var existingUser = await _userRepository.GetQueryable()
                        .Where(u => u.UserName == userName && u.Id != dto.UserID)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingUser == null;
                }).WithMessage("Username already exists."); // Added uniqueness check

            RuleFor(u => u.FullName)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .MustAsync(async (dto, email, cancellationToken) =>
                {
                    var existingUser = await _userRepository.GetQueryable()
                        .Where(u => u.Email == email && u.Id != dto.UserID)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingUser == null;
                }).WithMessage("Email already exists."); // Added uniqueness check

            RuleFor(u => u.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
        }
    }
}
