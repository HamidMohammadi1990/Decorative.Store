using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Discounts.Queries;

public class GetDiscountValidator : AbstractValidator<GetDiscountRequest>
{
    public GetDiscountValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
