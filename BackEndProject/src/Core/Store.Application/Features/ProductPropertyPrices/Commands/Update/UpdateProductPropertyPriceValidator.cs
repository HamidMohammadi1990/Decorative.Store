using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPropertyPrices.Commands;

public class UpdateProductPropertyPriceValidator : AbstractValidator<UpdateProductPropertyPriceRequest>
{
    public UpdateProductPropertyPriceValidator(IProductPropertyPriceRepository productPropertyPriceRepository)
    {
        RuleFor(x => new { x.Id, x.CompanyId, x.ProductPropertyId })
            .MustAsync(async (x, CancellationToken)
                   => !await productPropertyPriceRepository
                            .AnyAsync(p => p.Id != x.Id && p.CompanyId == x.CompanyId && p.ProductPropertyId == x.ProductPropertyId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}