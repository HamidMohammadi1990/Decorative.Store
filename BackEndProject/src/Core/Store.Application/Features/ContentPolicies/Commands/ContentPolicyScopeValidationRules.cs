using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ContentPolicies.Commands;

internal static class ContentPolicyScopeValidationRules
{
    public static void ApplyScopeRules<T>(
        AbstractValidator<T> validator,
        Func<T, int?> roleIdSelector,
        Func<T, int?> userIdSelector,
        IRoleRepository roleRepository,
        IUserRepository userRepository)
        where T : notnull
    {
        validator.RuleFor(x => x)
            .Must(x =>
            {
                var hasRole = roleIdSelector(x) is > 0;
                var hasUser = userIdSelector(x) is > 0;
                return hasRole ^ hasUser;
            })
            .WithMessage(MessageKeys.ContentPolicyScopeRequired);

        validator.When(x => roleIdSelector(x) is > 0, () =>
        {
            validator.RuleFor(x => roleIdSelector(x)!)
                .MustAsync(async (roleId, _) => await roleRepository.AnyAsync(x => x.Id == roleId))
                .WithMessage(MessageKeys.RoleNotFound);
        });

        validator.When(x => userIdSelector(x) is > 0, () =>
        {
            validator.RuleFor(x => userIdSelector(x)!)
                .MustAsync(async (userId, _) => await userRepository.AnyAsync(x => x.Id == userId))
                .WithMessage(MessageKeys.UserNotFound);
        });
    }
}
