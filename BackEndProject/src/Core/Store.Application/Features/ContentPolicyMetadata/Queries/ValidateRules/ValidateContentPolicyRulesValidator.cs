using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Domain.Enums;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class ValidateContentPolicyRulesValidator : AbstractValidator<ValidateContentPolicyRulesRequest>
{
    public ValidateContentPolicyRulesValidator(IContentEntityTypeRegistry entityTypeRegistry)
    {
        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleForEach(x => x.Rules)
            .ChildRules(rule =>
            {
                rule.RuleFor(r => r.FieldPath)
                    .NotEmpty()
                    .WithMessage(MessageKeys.InvalidRequest)
                    .MaximumLength(EntityFieldLengths.ContentPolicyRule.FieldPath)
                    .WithMessage(MessageKeys.MaxLength150Characters);

                rule.RuleFor(r => r.Value)
                    .MaximumLength(EntityFieldLengths.ContentPolicyRule.Value)
                    .WithMessage(MessageKeys.MaxLength200Characters);

                rule.RuleFor(r => r.Operator)
                    .IsInEnum()
                    .WithMessage(MessageKeys.InvalidRequest);

                rule.RuleFor(r => r.ValueType)
                    .IsInEnum()
                    .WithMessage(MessageKeys.InvalidRequest);

                rule.RuleFor(r => r.SortOrder)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage(MessageKeys.InvalidRequest);

                rule.RuleFor(r => r.RuleGroup)
                    .IsInEnum()
                    .WithMessage(MessageKeys.InvalidRequest);
            });
    }
}
