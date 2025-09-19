using FluentValidation;
using RMS.Application.DTOs.DiningTables;

namespace RMS.Application.Validators.DiningTableValidators
{
    public class UpdateDiningTableDtoValidator : AbstractValidator<UpdateDiningTableDto>
    {
        public UpdateDiningTableDtoValidator()
        {
            RuleFor(x => x.TableID).NotEmpty().WithMessage("Table ID is required.");
            RuleFor(x => x.TableName).NotEmpty().WithMessage("Table Name is required.").MaximumLength(100);
        }
    }
}