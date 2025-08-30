
using FluentValidation;
using RMS.Application.DTOs.SupplierDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.SupplierValidators
{
    public class UpdateSupplierDtoValidator : AbstractValidator<UpdateSupplierDto>
    {
        private readonly ISupplierRepository _supplierRepository;

        public UpdateSupplierDtoValidator(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.SupplierName)
                .NotEmpty().WithMessage("Supplier name is required.")
                .MaximumLength(100).WithMessage("Supplier name cannot exceed 100 characters.")
                .MustAsync(async (dto, supplierName, cancellationToken) =>
                {
                    var existingSupplier = await _supplierRepository.GetQueryable()
                        .Where(s => s.SupplierName == supplierName && s.Id != dto.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingSupplier == null;
                }).WithMessage("Supplier name already exists.");

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
    }
}
