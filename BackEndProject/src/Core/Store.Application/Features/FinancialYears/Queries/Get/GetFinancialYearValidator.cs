using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.FinancialYears.Queries;

public class GetFinancialYearValidator : AbstractValidator<GetFinancialYearRequest>
{
    public GetFinancialYearValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}