using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Roles.Queries;

public class GetRoleValidator : AbstractValidator<GetRoleRequest>
{
    public GetRoleValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
