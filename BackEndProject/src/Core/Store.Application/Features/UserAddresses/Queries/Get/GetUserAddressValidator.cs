using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetUserAddressValidator : AbstractValidator<GetUserAddressRequest>
{
    public GetUserAddressValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
