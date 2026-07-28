using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.RolePermissions.Commands;

public class CreateRolePermissionValidator : AbstractValidator<CreateRolePermissionRequest>
{
    public CreateRolePermissionValidator(
        IRoleRepository roleRepository,
        IPermissionRepository permissionRepository,
        IRolePermissionRepository rolePermissionRepository)
    {
        RuleFor(x => x.RoleId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRole)
            .MustAsync(async (roleId, _) => await roleRepository.AnyAsync(x => x.Id == roleId))
            .WithMessage(MessageKeys.RoleNotFound);

        RuleFor(x => x.PermissionId)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidPermission)
            .MustAsync(async (permissionId, _) => await permissionRepository.AnyAsync(x => x.Id == permissionId))
            .WithMessage(MessageKeys.PermissionNotFound);

        RuleFor(x => x)
            .MustAsync(async (request, _) =>
                !await rolePermissionRepository.AnyAsync(x =>
                    x.RoleId == request.RoleId && x.PermissionId == request.PermissionId))
            .WithMessage(MessageKeys.RolePermissionAlreadyAssigned);
    }
}
