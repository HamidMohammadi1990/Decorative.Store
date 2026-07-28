using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ContentPolicyRules.Commands;

public class DeleteContentPolicyRuleValidator : AbstractValidator<DeleteContentPolicyRuleRequest>
{
    public DeleteContentPolicyRuleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
