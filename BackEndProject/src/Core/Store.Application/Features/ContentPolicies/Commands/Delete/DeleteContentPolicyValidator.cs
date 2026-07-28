using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ContentPolicies.Commands;

public class DeleteContentPolicyValidator : AbstractValidator<DeleteContentPolicyRequest>
{
    public DeleteContentPolicyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
