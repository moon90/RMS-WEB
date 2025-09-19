using FluentValidation;
using RMS.Application.DTOs.DiningTables;

namespace RMS.Application.Validators.DiningTableValidators
{
    public class UpdateDiningTableStatusDtoValidator : AbstractValidator<UpdateDiningTableStatusDto>
    {
        public UpdateDiningTableStatusDtoValidator()
        {
            RuleFor(x => x.TableID).NotEmpty().WithMessage("Table ID is required.");
            RuleFor(x => x.Status).NotNull().WithMessage("Status is required.");
        }
    }
}