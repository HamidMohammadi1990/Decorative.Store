using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Users.Commands;

public class DeleteUserValidator : AbstractValidator<DeleteUserRequest>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
