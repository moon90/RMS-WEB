using FluentValidation;
using RMS.Application.DTOs.DiningTables;

namespace RMS.Application.Validators.DiningTableValidators
{
    public class CreateDiningTableDtoValidator : AbstractValidator<CreateDiningTableDto>
    {
        public CreateDiningTableDtoValidator()
        {
            RuleFor(x => x.TableName).NotEmpty().WithMessage("Table Name is required.").MaximumLength(100);
        }
    }
}