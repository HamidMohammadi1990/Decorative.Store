using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Commands;

public class UpdateProductPriceValidator : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceValidator(IProductPriceRepository productPriceRepository)
    {
        RuleFor(x => new { x.Id, x.CompanyId, x.ProductId })
            .MustAsync(async (x, CancellationToken)
                   => !await productPriceRepository
                            .AnyAsync(p => p.Id != x.Id && p.CompanyId == x.CompanyId && p.ProductId == x.ProductId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}