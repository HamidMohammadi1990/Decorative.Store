using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItemPrices.Commands;

public class CreatePropertyItemPriceValidator : AbstractValidator<CreatePropertyItemPriceRequest>
{
    public CreatePropertyItemPriceValidator(IPropertyItemPriceRepository propertyItemPriceRepository)
    {
        RuleFor(x => x.PropertyItemId)
            .MustAsync(async (propertyItemId, cancellationToken)
                   => !await propertyItemPriceRepository
                            .AnyAsync(p => p.PropertyItemId == propertyItemId && p.IsActive, cancellationToken))
            .WithMessage(MessageKeys.PriceAlreadyRegistered);
    }
}
