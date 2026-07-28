using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Roles.Commands;

public class DeleteRoleValidator : AbstractValidator<DeleteRoleRequest>
{
    public DeleteRoleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
