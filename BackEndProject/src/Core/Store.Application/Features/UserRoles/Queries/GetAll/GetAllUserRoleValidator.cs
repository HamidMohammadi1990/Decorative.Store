using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserRoles.Queries;

public class GetAllUserRoleValidator : AbstractValidator<GetAllUserRoleRequest>
{
    public GetAllUserRoleValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.RoleId).MustBeValidOptionalEntityId();
    }
}
