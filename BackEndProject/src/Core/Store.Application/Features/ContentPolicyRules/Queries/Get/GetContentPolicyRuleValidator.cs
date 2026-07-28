using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ContentPolicyRules.Queries;

public class GetContentPolicyRuleValidator : AbstractValidator<GetContentPolicyRuleRequest>
{
    public GetContentPolicyRuleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
