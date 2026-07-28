using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class CreatePropertyItemPriceValidator : AbstractValidator<CreatePropertyItemPriceRequest>
{
    public CreatePropertyItemPriceValidator(IPropertyItemPriceRepository propertyItemPriceRepository)
    {
        RuleFor(x => new { x.CompanyId, x.PropertyItemId })
            .MustAsync(async (x, CancellationToken)
                   => !await propertyItemPriceRepository
                            .AnyAsync(p => p.CompanyId == x.CompanyId && p.PropertyItemId == x.PropertyItemId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
