using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicyRules.Commands;

public class CreateContentPolicyRuleValidator : AbstractValidator<CreateContentPolicyRuleRequest>
{
    public CreateContentPolicyRuleValidator(IContentPolicyRepository contentPolicyRepository)
    {
        RuleFor(x => x.PolicyId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId)
            .MustAsync(async (policyId, cancellationToken) =>
                await contentPolicyRepository.FindAsync(policyId, cancellationToken) is not null)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.FieldPath)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(EntityFieldLengths.ContentPolicyRule.FieldPath)
            .WithMessage(MessageKeys.MaxLength150Characters);

        RuleFor(x => x.Value)
            .MaximumLength(EntityFieldLengths.ContentPolicyRule.Value)
            .WithMessage(MessageKeys.MaxLength200Characters);

        RuleFor(x => x.Operator)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ValueType)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.RuleGroup)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
