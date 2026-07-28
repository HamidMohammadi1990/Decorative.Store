using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class CreateProductPropertyPriceValidator : AbstractValidator<CreateProductPropertyPriceRequest>
{
    public CreateProductPropertyPriceValidator(IProductPropertyPriceRepository productPropertyPriceRepository)
    {
        RuleFor(x => new { x.CompanyId, x.ProductPropertyId })
            .MustAsync(async (x, CancellationToken)
                   => !await productPropertyPriceRepository
                            .AnyAsync(p => p.CompanyId == x.CompanyId && p.ProductPropertyId == x.ProductPropertyId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}