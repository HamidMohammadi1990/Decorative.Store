using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.FinancialYears.Commands;

public class DeleteFinancialYearValidator : AbstractValidator<DeleteFinancialYearRequest>
{
    public DeleteFinancialYearValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}