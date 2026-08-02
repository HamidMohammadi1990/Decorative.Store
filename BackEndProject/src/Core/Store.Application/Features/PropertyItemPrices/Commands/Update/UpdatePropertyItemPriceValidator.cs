using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class UpdatePropertyItemPriceValidator : AbstractValidator<UpdatePropertyItemPriceRequest>
{
    public UpdatePropertyItemPriceValidator(IPropertyItemPriceRepository propertyItemPriceRepository)
    {
        RuleFor(x => new { x.Id, x.PropertyItemId })
            .MustAsync(async (x, cancellationToken)
                   => !await propertyItemPriceRepository
                            .AnyAsync(p => p.Id != x.Id && p.PropertyItemId == x.PropertyItemId && p.IsActive, cancellationToken))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
