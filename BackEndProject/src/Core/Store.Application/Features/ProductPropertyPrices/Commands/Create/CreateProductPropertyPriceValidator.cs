using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class CreateProductPropertyPriceValidator : AbstractValidator<CreateProductPropertyPriceRequest>
{
    public CreateProductPropertyPriceValidator(IProductPropertyPriceRepository productPropertyPriceRepository)
    {
        RuleFor(x => x.ProductPropertyId)
            .MustAsync(async (productPropertyId, cancellationToken)
                   => !await productPropertyPriceRepository
                            .AnyAsync(p => p.ProductPropertyId == productPropertyId && p.IsActive, cancellationToken))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
