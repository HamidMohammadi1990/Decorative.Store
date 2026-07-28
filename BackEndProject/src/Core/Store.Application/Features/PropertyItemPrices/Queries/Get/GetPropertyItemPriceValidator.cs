using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public class GetPropertyItemPriceValidator : AbstractValidator<GetPropertyItemPriceRequest>
{
    public GetPropertyItemPriceValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
