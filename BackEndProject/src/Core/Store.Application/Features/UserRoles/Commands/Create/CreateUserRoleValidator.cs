using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.UserRoles.Commands;

public class CreateUserRoleValidator : AbstractValidator<CreateUserRoleRequest>
{
    public CreateUserRoleValidator(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository)
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidUser)
            .MustAsync(async (userId, cancellationToken) =>
                await userRepository.FindAsync(userId, cancellationToken) is not null)
            .WithMessage(MessageKeys.UserNotFound);

        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRole)
            .MustAsync(async (roleId, _) => await roleRepository.AnyAsync(x => x.Id == roleId))
            .WithMessage(MessageKeys.RoleNotFound);

        RuleFor(x => x)
            .MustAsync(async (request, _) =>
                !await userRoleRepository.AnyAsync(x =>
                    x.UserId == request.UserId && x.RoleId == request.RoleId))
            .WithMessage(MessageKeys.UserRoleAlreadyAssigned);
    }
}
