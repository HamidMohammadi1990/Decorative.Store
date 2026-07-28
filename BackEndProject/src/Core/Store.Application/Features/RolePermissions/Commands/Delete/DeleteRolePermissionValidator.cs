using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.RolePermissions.Commands;

public class DeleteRolePermissionValidator : AbstractValidator<DeleteRolePermissionRequest>
{
    public DeleteRolePermissionValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
