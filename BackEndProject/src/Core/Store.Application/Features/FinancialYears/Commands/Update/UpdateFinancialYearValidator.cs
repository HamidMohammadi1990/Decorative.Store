using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.FinancialYears.Commands;

public class UpdateFinancialYearValidator : AbstractValidator<UpdateFinancialYearRequest>
{
    public UpdateFinancialYearValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidIdValidator);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.NameRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters)
            .MinimumLength(10)
            .WithMessage(MessageKeys.MinLength10Characters); ;

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage(MessageKeys.StartDateMustBeBeforeEndDate);
    }
}