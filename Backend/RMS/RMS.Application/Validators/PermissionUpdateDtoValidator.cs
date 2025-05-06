using FluentValidation;
using RMS.Domain.Dtos.PermissionDTOs.InputDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Application.Validators
{
    public class PermissionUpdateDtoValidator : AbstractValidator<PermissionUpdateDto>
    {
        public PermissionUpdateDtoValidator()
        {
            RuleFor(p => p.Id)
                .GreaterThan(0).WithMessage("Permission ID must be greater than 0.");

            RuleFor(p => p.PermissionName)
                .NotEmpty().WithMessage("Permission name is required.")
                .MaximumLength(100).WithMessage("Permission name cannot exceed 100 characters.");

            RuleFor(p => p.PermissionKey)
                .NotEmpty().WithMessage("Permission key is required.")
                .MaximumLength(100).WithMessage("Permission key cannot exceed 100 characters.");

            RuleFor(p => p.ControllerName)
                .MaximumLength(100).WithMessage("Controller name cannot exceed 100 characters.");

            RuleFor(p => p.ActionName)
                .MaximumLength(100).WithMessage("Action name cannot exceed 100 characters.");

            RuleFor(p => p.ModuleName)
                .MaximumLength(100).WithMessage("Module name cannot exceed 100 characters.");
        }
    }
}
