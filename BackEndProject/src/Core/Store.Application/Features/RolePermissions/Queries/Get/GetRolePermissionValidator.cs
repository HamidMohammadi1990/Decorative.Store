using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.RolePermissions.Queries;

public class GetRolePermissionValidator : AbstractValidator<GetRolePermissionRequest>
{
    public GetRolePermissionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
