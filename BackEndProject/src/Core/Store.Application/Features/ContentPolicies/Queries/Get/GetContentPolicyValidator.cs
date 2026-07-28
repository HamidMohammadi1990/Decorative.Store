using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ContentPolicies.Queries;

public class GetContentPolicyValidator : AbstractValidator<GetContentPolicyRequest>
{
    public GetContentPolicyValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
