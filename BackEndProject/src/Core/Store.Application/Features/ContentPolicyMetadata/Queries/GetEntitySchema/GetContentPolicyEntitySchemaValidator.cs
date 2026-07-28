using FluentValidation;
using Edition.Application.Common.Validation;
using Edition.Application.Features.ContentPolicies.Commands;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicyMetadata.Queries;

public class GetContentPolicyEntitySchemaValidator : AbstractValidator<GetContentPolicyEntitySchemaRequest>
{
    public GetContentPolicyEntitySchemaValidator(IContentEntityTypeRegistry entityTypeRegistry)
    {
        ContentPolicyEntityTypeValidationRules.ApplyRequiredEntityTypeRules(
            this,
            x => x.EntityType,
            entityTypeRegistry);

        RuleFor(x => x.ParentPath)
            .MaximumLength(EntityFieldLengths.ContentPolicyRule.FieldPath)
            .WithMessage(MessageKeys.MaxLength150Characters);
    }
}
