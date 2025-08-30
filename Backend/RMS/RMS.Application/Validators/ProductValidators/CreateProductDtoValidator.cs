
using FluentValidation;
using RMS.Application.DTOs.ProductDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.ProductValidators
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductDtoValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.")
                .MustAsync(BeUniqueProductName).WithMessage("Product name already exists.");

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Cost price cannot be negative.")
                .LessThanOrEqualTo(x => x.ProductPrice).WithMessage("Cost price cannot be greater than product price.")
                .When(x => x.CostPrice.HasValue);

            RuleFor(x => x.ProductBarcode)
                .MaximumLength(100).WithMessage("Product barcode cannot exceed 100 characters.")
                .MustAsync(BeUniqueProductBarcode).WithMessage("Product barcode already exists.")
                .When(x => !string.IsNullOrEmpty(x.ProductBarcode));

            RuleFor(x => x.CategoryID)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.")
                .When(x => x.CategoryID.HasValue);

            RuleFor(x => x.SupplierID)
                .GreaterThan(0).WithMessage("Supplier ID must be greater than 0.")
                .When(x => x.SupplierID.HasValue);

            RuleFor(x => x.ManufacturerID)
                .GreaterThan(0).WithMessage("Manufacturer ID must be greater than 0.")
                .When(x => x.ManufacturerID.HasValue);

            RuleFor(x => x.ExpireDate)
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Expire date cannot be in the past.")
                .When(x => x.ExpireDate.HasValue);
        }

        private async Task<bool> BeUniqueProductName(string productName, CancellationToken cancellationToken)
        {
            return await _productRepository.GetQueryable().AllAsync(p => p.ProductName != productName, cancellationToken);
        }

        private async Task<bool> BeUniqueProductBarcode(string? productBarcode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(productBarcode))
            {
                return true; // Barcode is optional, so if it's empty, it's unique.
            }
            return await _productRepository.GetQueryable().AllAsync(p => p.ProductBarcode != productBarcode, cancellationToken);
        }
    }
}
