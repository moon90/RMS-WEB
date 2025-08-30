using FluentValidation;
using RMS.Application.DTOs.UnitDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.UnitValidators
{
    public class UpdateUnitDtoValidator : AbstractValidator<UpdateUnitDto>
    {
        private readonly IUnitRepository _unitRepository;

        public UpdateUnitDtoValidator(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Unit ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Unit name is required.")
                .MaximumLength(50).WithMessage("Unit name cannot exceed 50 characters.")
                .MustAsync(async (dto, name, cancellationToken) =>
                {
                    var existingUnit = await _unitRepository.GetQueryable()
                        .Where(u => u.Name == name && u.Id != dto.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingUnit == null;
                }).WithMessage("Unit name already exists.");

            RuleFor(x => x.ShortCode)
                .NotEmpty().WithMessage("Short code is required.")
                .MaximumLength(10).WithMessage("Short code cannot exceed 10 characters.")
                .MustAsync(async (dto, shortCode, cancellationToken) =>
                {
                    var existingUnit = await _unitRepository.GetQueryable()
                        .Where(u => u.ShortCode == shortCode && u.Id != dto.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    return existingUnit == null;
                }).WithMessage("Short code already exists.");
        }
    }
}
