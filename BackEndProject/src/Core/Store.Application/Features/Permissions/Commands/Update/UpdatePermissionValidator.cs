using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Permissions.Commands;

public class UpdatePermissionValidator : AbstractValidator<UpdatePermissionRequest>
{
    public UpdatePermissionValidator()
    {
        RuleFor(x => x.Id)
            .IsInEnum()
            .WithMessage(MessageKeys.PermissionIdInvalid);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage(MessageKeys.AddressRequired);

        RuleFor(x => x.NameSpace)
            .NotEmpty()
            .WithMessage(MessageKeys.PermissionNamespaceRequired);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.PermissionPriorityInvalid);
    }
}
