using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Roles.Queries;

public class GetAllRoleValidator : AbstractValidator<GetAllRoleRequest>
{
    public GetAllRoleValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Role.Name);
    }
}
