using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Commands;

public class UpdateProductPriceDeliveryOptionValidator : AbstractValidator<UpdateProductPriceDeliveryOptionRequest>
{
    public UpdateProductPriceDeliveryOptionValidator(IProductPriceDeliveryOptionRepository productPriceDeliveryOptionRepository)
    {
        RuleFor(x => new { x.Id, x.ProductPriceId, x.DeliveryOptionId })
            .MustAsync(async (x, CancellationToken)
             => !await productPriceDeliveryOptionRepository
            .AnyAsync(c => c.ProductPriceId == x.ProductPriceId && c.DeliveryOptionId == x.DeliveryOptionId && c.Id != x.Id))
            .WithMessage(MessageKeys.DuplicateDelivery);
    }
}