using FluentValidation;
using RMS.Application.DTOs.CustomerDTOs.InputDTOs;

namespace RMS.Application.Validators.CustomerValidators
{
    public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerDtoValidator()
        {
            RuleFor(x => x.CustomerID)
                .NotEmpty().WithMessage("Customer ID is required.");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer Name is required.")
                .MaximumLength(100).WithMessage("Customer Name cannot exceed 100 characters.");

            RuleFor(x => x.CustomerPhone)
                .MaximumLength(20).WithMessage("Customer Phone cannot exceed 20 characters.");

            RuleFor(x => x.CustomerEmail)
                .MaximumLength(100).WithMessage("Customer Email cannot exceed 100 characters.")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.CustomerEmail)).WithMessage("Invalid Email Address.");

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");

            RuleFor(x => x.DriverName)
                .MaximumLength(100).WithMessage("Driver Name cannot exceed 100 characters.");
        }
    }
}
