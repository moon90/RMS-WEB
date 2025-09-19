using FluentValidation;
using RMS.Application.DTOs.InventoryDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.InventoryValidators
{
    public class CreateInventoryDtoValidator : AbstractValidator<CreateInventoryDto>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public CreateInventoryDtoValidator(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;

            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.")
                .MustAsync(BeUniqueProductInventory).WithMessage("Inventory for this product already exists.");

            RuleFor(x => x.InitialStock)
                .GreaterThanOrEqualTo(0).WithMessage("Initial Stock cannot be negative.");

            RuleFor(x => x.MinStockLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum Stock Level cannot be negative.");
        }

        private async Task<bool> BeUniqueProductInventory(int productID, CancellationToken cancellationToken)
        {
            return await _inventoryRepository.GetQueryable().AllAsync(i => i.ProductID != productID, cancellationToken);
        }
    }
}
