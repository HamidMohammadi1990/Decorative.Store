using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class DeleteProductPriceDeliveryOptionValidator : AbstractValidator<DeleteProductPriceDeliveryOptionRequest>
{
    public DeleteProductPriceDeliveryOptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
