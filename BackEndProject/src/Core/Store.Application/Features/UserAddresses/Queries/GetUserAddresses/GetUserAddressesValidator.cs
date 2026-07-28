using FluentValidation;
using Store.Common.Localization;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.UserAddresses.Queries;

public class GetUserAddressesValidator : AbstractValidator<GetUserAddressesRequest>
{
    public GetUserAddressesValidator()
    {
        RuleFor(x => x.Pagination)
            .MustBeValidPagination();
    }
}
