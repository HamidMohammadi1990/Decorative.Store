using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Permissions.Queries;

public class GetPermissionValidator : AbstractValidator<GetPermissionRequest>
{
    public GetPermissionValidator()
    {
        RuleFor(x => x.Id)
            .IsInEnum()
            .WithMessage(MessageKeys.PermissionIdInvalid);
    }
}
