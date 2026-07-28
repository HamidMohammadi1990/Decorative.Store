using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetAllProductPropertyRuleValidator : AbstractValidator<GetAllProductPropertyRuleRequest>
{
    public GetAllProductPropertyRuleValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductPropertyId).MustBeValidOptionalEntityId();
    }
}
