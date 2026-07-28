using FluentValidation;
using Store.Common.Localization;
using Store.Domain.ContentPolicies;

namespace Edition.Application.Features.ContentPolicies.Commands;

internal static class ContentPolicyEntityTypeValidationRules
{
    public static void ApplyRequiredEntityTypeRules<T>(
        AbstractValidator<T> validator,
        Func<T, string> entityTypeSelector,
        IContentEntityTypeRegistry entityTypeRegistry)
    {
        validator.RuleFor(x => entityTypeSelector(x))
            .NotEmpty()
            .WithMessage(MessageKeys.ContentPolicyEntityTypeRequired)
            .Must(entityTypeRegistry.IsRegistered)
            .WithMessage(MessageKeys.ContentPolicyEntityTypeNotRegistered);
    }

    public static void ApplyOptionalEntityTypeRules<T>(
        AbstractValidator<T> validator,
        Func<T, string?> entityTypeSelector,
        IContentEntityTypeRegistry entityTypeRegistry)
    {
        validator.When(x => entityTypeSelector(x) is not null, () =>
        {
            validator.RuleFor(x => entityTypeSelector(x)!)
                .NotEmpty()
                .WithMessage(MessageKeys.ContentPolicyEntityTypeRequired)
                .Must(entityTypeRegistry.IsRegistered)
                .WithMessage(MessageKeys.ContentPolicyEntityTypeNotRegistered);
        });
    }
}
