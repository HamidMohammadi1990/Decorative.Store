using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyPropertyOperatorsValidator : AbstractValidator<GetContentPolicyPropertyOperatorsRequest>
{
    public GetContentPolicyPropertyOperatorsValidator(IContentEntityTypeRegistry entityTypeRegistry)
    {
        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.FieldPath)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(EntityFieldLengths.ContentPolicyRule.FieldPath)
            .WithMessage(MessageKeys.MaxLength150Characters);
    }
}
