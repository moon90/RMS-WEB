using FluentValidation;
using RMS.Application.DTOs.StaffDTOs.InputDTOs;

namespace RMS.Application.Validators.StaffValidators
{
    public class UpdateStaffDtoValidator : AbstractValidator<UpdateStaffDto>
    {
        public UpdateStaffDtoValidator()
        {
            RuleFor(x => x.StaffID)
                .NotEmpty().WithMessage("Staff ID is required.");

            RuleFor(x => x.StaffName)
                .NotEmpty().WithMessage("Staff Name is required.")
                .MaximumLength(100).WithMessage("Staff Name cannot exceed 100 characters.");

            RuleFor(x => x.StaffPhone)
                .MaximumLength(20).WithMessage("Staff Phone cannot exceed 20 characters.");

            RuleFor(x => x.StaffRole)
                .MaximumLength(100).WithMessage("Staff Role cannot exceed 100 characters.");
        }
    }
}
