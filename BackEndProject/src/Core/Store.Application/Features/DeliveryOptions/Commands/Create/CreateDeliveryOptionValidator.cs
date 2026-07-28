using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public class CreateDeliveryOptionValidator : AbstractValidator<CreateDeliveryOptionRequest>
{
    public CreateDeliveryOptionValidator(IDeliveryOptionRepository deliveryOptionRepository)
    {
        RuleFor(x => x.Title)
             .MustAsync(async (x, CancellationToken)
                    => !await deliveryOptionRepository.AnyAsync(c => c.Title == x.Trim()))
             .WithMessage(MessageKeys.DuplicateTitleOrAddress)
             .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.DeliveryDays)
            .GreaterThan(0)
            .WithMessage(MessageKeys.MinDeliveryDays1);
    }
}