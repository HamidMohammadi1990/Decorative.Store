using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.Permissions.Queries;

public class GetAllPermissionValidator : AbstractValidator<GetAllPermissionRequest>
{
    public GetAllPermissionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ParentId)
            .IsInEnum()
            .When(x => x.ParentId.HasValue)
            .WithMessage(MessageKeys.PermissionIdInvalid);
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Permission.Title);
        RuleFor(x => x.Url).MaximumLengthWhenNotEmpty(EntityFieldLengths.Permission.Slug);
        RuleFor(x => x.NameSpace).MaximumLengthWhenNotEmpty(EntityFieldLengths.Permission.GroupName);
    }
}
