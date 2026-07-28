using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Permissions.Commands;

public class DeletePermissionValidator : AbstractValidator<DeletePermissionRequest>
{
    public DeletePermissionValidator()
    {
        RuleFor(x => x.Id)
            .IsInEnum()
            .WithMessage(MessageKeys.PermissionIdInvalid);
    }
}
