using FluentValidation;
using RMS.Application.DTOs.InventoryDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace RMS.Application.Validators.InventoryValidators
{
    public class UpdateInventoryDtoValidator : AbstractValidator<UpdateInventoryDto>
    {
        private readonly IInventoryRepository _inventoryRepository;

        public UpdateInventoryDtoValidator(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;

            RuleFor(x => x.InventoryID)
                .NotEmpty().WithMessage("Inventory ID is required.");

            RuleFor(x => x.ProductID)
                .NotEmpty().WithMessage("Product ID is required.")
                .MustAsync(async (dto, productID, cancellationToken) =>
                {
                    var existingInventory = await _inventoryRepository.GetQueryable()
                        .FirstOrDefaultAsync(i => i.ProductID == productID && i.InventoryID != dto.InventoryID, cancellationToken);
                    return existingInventory == null;
                }).WithMessage("Inventory for this product already exists.");

            RuleFor(x => x.InitialStock)
                .GreaterThanOrEqualTo(0).WithMessage("Initial Stock cannot be negative.");

            RuleFor(x => x.MinStockLevel)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum Stock Level cannot be negative.");
        }
    }
}
