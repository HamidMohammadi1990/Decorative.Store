using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class CreateProductPriceDeliveryOptionValidator : AbstractValidator<CreateProductPriceDeliveryOptionRequest>
{
    public CreateProductPriceDeliveryOptionValidator(IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository)
    {
        RuleFor(x => new { x.ProductPriceId, x.DeliveryOptionId })
            .MustAsync(async (x, CancellationToken)
                   => !await productPriceDeliveryOptionRepository
                   .AnyAsync(c => c.ProductPriceId == x.ProductPriceId && c.DeliveryOptionId == x.DeliveryOptionId))
                   .WithMessage(MessageKeys.DuplicateDelivery);
    }
}