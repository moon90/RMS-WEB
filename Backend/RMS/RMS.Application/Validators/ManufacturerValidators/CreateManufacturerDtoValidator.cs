
using FluentValidation;
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.ManufacturerValidators
{
    public class CreateManufacturerDtoValidator : AbstractValidator<CreateManufacturerDto>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public CreateManufacturerDtoValidator(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;

            RuleFor(x => x.ManufacturerName)
                .NotEmpty().WithMessage("Manufacturer name is required.")
                .MaximumLength(100).WithMessage("Manufacturer name cannot exceed 100 characters.")
                .MustAsync(BeUniqueManufacturerName).WithMessage("Manufacturer name already exists.");
        }

        private async Task<bool> BeUniqueManufacturerName(string manufacturerName, CancellationToken cancellationToken)
        {
            return await _manufacturerRepository.GetQueryable().AllAsync(m => m.ManufacturerName != manufacturerName, cancellationToken);
        }
    }
}
