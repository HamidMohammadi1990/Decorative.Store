using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Common.Validation;

public static class IdValidationExtensions
{
    public static IRuleBuilderOptions<T, int> MustBeValidEntityId<T>(this IRuleBuilder<T, int> ruleBuilder)
        => ruleBuilder
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId);

    public static IRuleBuilderOptions<T, int?> MustBeValidOptionalEntityId<T>(this IRuleBuilder<T, int?> ruleBuilder)
        => ruleBuilder
            .Must(id => id is null or > 0)
            .WithMessage(MessageKeys.InvalidId);

    public static IRuleBuilderOptions<T, long> MustBeValidEntityId<T>(this IRuleBuilder<T, long> ruleBuilder)
        => ruleBuilder
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId);

    public static IRuleBuilderOptions<T, long?> MustBeValidOptionalEntityId<T>(this IRuleBuilder<T, long?> ruleBuilder)
        => ruleBuilder
            .Must(id => id is null or > 0)
            .WithMessage(MessageKeys.InvalidId);
}
