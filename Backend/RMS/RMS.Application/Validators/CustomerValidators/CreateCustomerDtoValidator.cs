using FluentValidation;
using RMS.Application.DTOs.CustomerDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.CustomerValidators
{
    public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerDtoValidator(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer Name is required.")
                .MaximumLength(100).WithMessage("Customer Name cannot exceed 100 characters.")
                .MustAsync(BeUniqueCustomerName).WithMessage("A customer with this name already exists.");

            RuleFor(x => x.CustomerPhone)
                .MaximumLength(20).WithMessage("Customer Phone cannot exceed 20 characters.");

            RuleFor(x => x.CustomerEmail)
                .MaximumLength(100).WithMessage("Customer Email cannot exceed 100 characters.")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.CustomerEmail)).WithMessage("Invalid Email Address.")
                .MustAsync(BeUniqueCustomerEmail).WithMessage("A customer with this email already exists.").When(x => !string.IsNullOrEmpty(x.CustomerEmail));

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");

            RuleFor(x => x.DriverName)
                .MaximumLength(100).WithMessage("Driver Name cannot exceed 100 characters.");
        }

        private async Task<bool> BeUniqueCustomerName(string customerName, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetQueryable().AllAsync(c => c.CustomerName != customerName, cancellationToken);
        }

        private async Task<bool> BeUniqueCustomerEmail(string customerEmail, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetQueryable().AllAsync(c => c.CustomerEmail != customerEmail, cancellationToken);
        }
    }
}
