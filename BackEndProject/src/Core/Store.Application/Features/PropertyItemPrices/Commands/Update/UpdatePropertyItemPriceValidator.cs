using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class UpdatePropertyItemPriceValidator : AbstractValidator<UpdatePropertyItemPriceRequest>
{
    public UpdatePropertyItemPriceValidator(IPropertyItemPriceRepository propertyItemPriceRepository)
    {
        RuleFor(x => new { x.Id, x.CompanyId, x.PropertyItemId })
            .MustAsync(async (x, CancellationToken)
                   => !await propertyItemPriceRepository
                            .AnyAsync(p => p.Id != x.Id && p.CompanyId == x.CompanyId && p.PropertyItemId == x.PropertyItemId && p.IsActive))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
