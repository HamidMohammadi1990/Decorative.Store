using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class UpdateProductPropertyPriceValidator : AbstractValidator<UpdateProductPropertyPriceRequest>
{
    public UpdateProductPropertyPriceValidator(IProductPropertyPriceRepository productPropertyPriceRepository)
    {
        RuleFor(x => new { x.Id, x.ProductPropertyId })
            .MustAsync(async (x, cancellationToken)
                   => !await productPropertyPriceRepository
                            .AnyAsync(p => p.Id != x.Id && p.ProductPropertyId == x.ProductPropertyId && p.IsActive, cancellationToken))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
