using FluentValidation;
using RMS.Application.DTOs.StaffDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.StaffValidators
{
    public class CreateStaffDtoValidator : AbstractValidator<CreateStaffDto>
    {
        private readonly IStaffRepository _staffRepository;

        public CreateStaffDtoValidator(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;

            RuleFor(x => x.StaffName)
                .NotEmpty().WithMessage("Staff Name is required.")
                .MaximumLength(100).WithMessage("Staff Name cannot exceed 100 characters.")
                .MustAsync(BeUniqueStaffName).WithMessage("A staff member with this name already exists.");

            RuleFor(x => x.StaffPhone)
                .MaximumLength(20).WithMessage("Staff Phone cannot exceed 20 characters.")
                .MustAsync(BeUniqueStaffPhone).WithMessage("A staff member with this phone number already exists.").When(x => !string.IsNullOrEmpty(x.StaffPhone));

            RuleFor(x => x.StaffRole)
                .MaximumLength(100).WithMessage("Staff Role cannot exceed 100 characters.");
        }

        private async Task<bool> BeUniqueStaffName(string staffName, CancellationToken cancellationToken)
        {
            return await _staffRepository.GetQueryable().AllAsync(s => s.StaffName != staffName, cancellationToken);
        }

        private async Task<bool> BeUniqueStaffPhone(string staffPhone, CancellationToken cancellationToken)
        {
            return await _staffRepository.GetQueryable().AllAsync(s => s.StaffPhone != staffPhone, cancellationToken);
        }
    }
}
