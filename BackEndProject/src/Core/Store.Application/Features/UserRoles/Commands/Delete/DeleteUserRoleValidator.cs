using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.UserRoles.Commands;

public class DeleteUserRoleValidator : AbstractValidator<DeleteUserRoleRequest>
{
    public DeleteUserRoleValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
