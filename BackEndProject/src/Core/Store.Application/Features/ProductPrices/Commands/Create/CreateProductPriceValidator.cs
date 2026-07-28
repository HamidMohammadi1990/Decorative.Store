using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductPrices.Commands;

public class CreateProductPriceValidator : AbstractValidator<CreateProductPriceRequest>
{
    public CreateProductPriceValidator(IProductPriceRepository productPriceRepository)
    {
        RuleFor(x => new { x.CompanyId, x.ProductId })
            .MustAsync(async (x, CancellationToken)
                   => !await productPriceRepository.AnyAsync(p => p.CompanyId == x.CompanyId && p.ProductId == x.ProductId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}