using FluentValidation;
using RMS.Application.DTOs.UnitDTOs.InputDTOs;
using RMS.Infrastructure.IRepositories;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RMS.Application.Validators.UnitValidators
{
    public class CreateUnitDtoValidator : AbstractValidator<CreateUnitDto>
    {
        private readonly IUnitRepository _unitRepository;

        public CreateUnitDtoValidator(IUnitRepository unitRepository)
        {
            _unitRepository = unitRepository;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Unit name is required.")
                .MaximumLength(50).WithMessage("Unit name cannot exceed 50 characters.")
                .MustAsync(BeUniqueUnitName).WithMessage("Unit name already exists.");

            RuleFor(x => x.ShortCode)
                .NotEmpty().WithMessage("Short code is required.")
                .MaximumLength(10).WithMessage("Short code cannot exceed 10 characters.")
                .MustAsync(BeUniqueShortCode).WithMessage("Short code already exists.");
        }

        private async Task<bool> BeUniqueUnitName(string name, CancellationToken cancellationToken)
        {
            return await _unitRepository.GetQueryable().AllAsync(u => u.Name != name, cancellationToken);
        }

        private async Task<bool> BeUniqueShortCode(string shortCode, CancellationToken cancellationToken)
        {
            return await _unitRepository.GetQueryable().AllAsync(u => u.ShortCode != shortCode, cancellationToken);
        }
    }
}
