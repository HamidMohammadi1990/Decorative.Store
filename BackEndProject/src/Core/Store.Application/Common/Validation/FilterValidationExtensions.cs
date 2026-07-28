using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Common.Validation;

public static class FilterValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> MaximumLengthWhenNotEmpty<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int maxLength)
        => ruleBuilder
            .Must(value => string.IsNullOrWhiteSpace(value) || value.Length <= maxLength)
            .WithMessage(MessageKeys.InvalidRequest);
}
