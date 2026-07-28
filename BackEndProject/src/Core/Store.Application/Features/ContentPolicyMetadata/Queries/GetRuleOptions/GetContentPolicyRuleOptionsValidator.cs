using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyRuleOptionsValidator : AbstractValidator<GetContentPolicyRuleOptionsRequest>
{
    public GetContentPolicyRuleOptionsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
