using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public class GetChartOfAccountValidator : AbstractValidator<GetChartOfAccountRequest>
{
    public GetChartOfAccountValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
