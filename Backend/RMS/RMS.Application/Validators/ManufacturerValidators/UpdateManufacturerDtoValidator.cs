
using FluentValidation;
using RMS.Application.DTOs.ManufacturerDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.ManufacturerValidators
{
    public class UpdateManufacturerDtoValidator : AbstractValidator<UpdateManufacturerDto>
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public UpdateManufacturerDtoValidator(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.ManufacturerName)
                .NotEmpty().WithMessage("Manufacturer name is required.")
                .MaximumLength(100).WithMessage("Manufacturer name cannot exceed 100 characters.")
                .MustAsync(async (dto, manufacturerName, cancellationToken) =>
                {
                    var existingManufacturer = await _manufacturerRepository.GetQueryable()
                        .Where(m => m.ManufacturerName == manufacturerName && m.Id != dto.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingManufacturer == null;
                }).WithMessage("Manufacturer name already exists.");
        }
    }
}
