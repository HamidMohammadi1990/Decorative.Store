using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPrices.Queries;

public class GetProductPriceValidator : AbstractValidator<GetProductPriceRequest>
{
    public GetProductPriceValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
