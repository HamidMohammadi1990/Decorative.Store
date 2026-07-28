using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public class GetProductPriceDeliveryOptionValidator : AbstractValidator<GetProductPriceDeliveryOptionRequest>
{
    public GetProductPriceDeliveryOptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
