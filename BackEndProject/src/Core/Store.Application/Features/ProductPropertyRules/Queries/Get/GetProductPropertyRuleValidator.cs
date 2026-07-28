using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyRules.Queries;

public class GetProductPropertyRuleValidator : AbstractValidator<GetProductPropertyRuleRequest>
{
    public GetProductPropertyRuleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
