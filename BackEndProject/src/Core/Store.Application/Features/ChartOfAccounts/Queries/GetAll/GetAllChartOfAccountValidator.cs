using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ChartOfAccounts.Queries;

public class GetAllChartOfAccountValidator : AbstractValidator<GetAllChartOfAccountRequest>
{
    public GetAllChartOfAccountValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ParentId).MustBeValidOptionalEntityId();
        RuleFor(x => x.AccountCode).MaximumLengthWhenNotEmpty(EntityFieldLengths.ChartOfAccount.Code);
        RuleFor(x => x.AccountTitle).MaximumLengthWhenNotEmpty(EntityFieldLengths.ChartOfAccount.Title);
    }
}
