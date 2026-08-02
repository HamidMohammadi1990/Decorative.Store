using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.FinancialYears.Commands;

public class CreateFinancialYearValidator : AbstractValidator<CreateFinancialYearRequest>
{
    public CreateFinancialYearValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.NameRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters)
            .MinimumLength(10)
            .WithMessage(MessageKeys.MinLength10Characters);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage(MessageKeys.StartDateMustBeBeforeEndDate);
    }
}
