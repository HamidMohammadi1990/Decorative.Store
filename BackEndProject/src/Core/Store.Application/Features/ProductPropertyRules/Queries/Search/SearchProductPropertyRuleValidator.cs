using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class SearchProductPropertyRuleValidator : AbstractValidator<SearchProductPropertyRuleRequest>
{
    public SearchProductPropertyRuleValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductPropertyId).MustBeValidOptionalEntityId();
    }
}
