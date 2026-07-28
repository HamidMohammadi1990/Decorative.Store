using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserAddresses.Commands;

public class DeleteUserAddressValidator : AbstractValidator<DeleteUserAddressRequest>
{
    public DeleteUserAddressValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
