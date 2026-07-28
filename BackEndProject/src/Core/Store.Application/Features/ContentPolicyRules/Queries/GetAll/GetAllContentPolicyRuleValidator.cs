using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyRules.Queries;

public class GetAllContentPolicyRuleValidator : AbstractValidator<GetAllContentPolicyRuleRequest>
{
    public GetAllContentPolicyRuleValidator(IContentEntityTypeRegistry entityTypeRegistry)
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.PolicyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.FieldPath).MaximumLengthWhenNotEmpty(EntityFieldLengths.ContentPolicyRule.FieldPath);

        ContentPolicyEntityTypeValidationRules.ApplyOptionalEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.RuleGroup)
            .IsInEnum()
            .When(x => x.RuleGroup.HasValue)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
