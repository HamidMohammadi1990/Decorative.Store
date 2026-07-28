using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Permissions.Queries;

public class HasPermissionValidator : AbstractValidator<HasPermissionRequest>
{
    public HasPermissionValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidUser);

        RuleFor(x => x.PermissionType)
            .NotNull()
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidPermission);
    }
}