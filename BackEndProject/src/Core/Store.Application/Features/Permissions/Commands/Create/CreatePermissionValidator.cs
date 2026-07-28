using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Permissions.Commands;

public class CreatePermissionValidator : AbstractValidator<CreatePermissionRequest>
{
    public CreatePermissionValidator(IPermissionRepository permissionRepository)
    {
        RuleFor(x => x.Id)
            .IsInEnum()
            .WithMessage(MessageKeys.PermissionIdInvalid)
            .MustAsync(async (id, _) => !await permissionRepository.AnyAsync(x => x.Id == id))
            .WithMessage(MessageKeys.DuplicatePermissionId);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Url)
            .NotEmpty()
            .WithMessage(MessageKeys.AddressRequired);

        RuleFor(x => x.NameSpace)
            .NotEmpty()
            .WithMessage(MessageKeys.PermissionNamespaceRequired);

        RuleFor(x => x.LevelTypeId)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidPermissionLevel);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.PermissionPriorityInvalid);

        RuleFor(x => x.ParentId)
            .MustAsync(async (parentId, _) =>
            {
                if (!parentId.HasValue)
                    return true;

                return await permissionRepository.AnyAsync(x => x.Id == parentId.Value);
            })
            .When(x => x.ParentId.HasValue)
            .WithMessage(MessageKeys.InvalidParentId);
    }
}
