using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserRoles.Queries;

public class GetUserRoleValidator : AbstractValidator<GetUserRoleRequest>
{
    public GetUserRoleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
