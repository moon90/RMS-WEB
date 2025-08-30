
using FluentValidation;
using RMS.Application.DTOs.SupplierDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.SupplierValidators
{
    public class CreateSupplierDtoValidator : AbstractValidator<CreateSupplierDto>
    {
        private readonly ISupplierRepository _supplierRepository;

        public CreateSupplierDtoValidator(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;

            RuleFor(x => x.SupplierName)
                .NotEmpty().WithMessage("Supplier name is required.")
                .MaximumLength(100).WithMessage("Supplier name cannot exceed 100 characters.")
                .MustAsync(BeUniqueSupplierName).WithMessage("Supplier name already exists.");

            RuleFor(x => x.ContactPerson)
                .MaximumLength(100).WithMessage("Contact person cannot exceed 100 characters.");

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email address.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Address)
                .MaximumLength(250).WithMessage("Address cannot exceed 250 characters.");
        }

        private async Task<bool> BeUniqueSupplierName(string supplierName, CancellationToken cancellationToken)
        {
            return await _supplierRepository.GetQueryable().AllAsync(s => s.SupplierName != supplierName, cancellationToken);
        }
    }
}
