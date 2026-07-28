using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetAllUserAddressValidator : AbstractValidator<GetAllUserAddressRequest>
{
    public GetAllUserAddressValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CityId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.User.Address);
        RuleFor(x => x.PostalCode).MaximumLengthWhenNotEmpty(EntityFieldLengths.Company.PostalCode);
    }
}
