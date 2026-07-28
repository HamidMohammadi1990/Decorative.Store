using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryOptions.Commands;

public class UpdateDeliveryOptionValidator : AbstractValidator<UpdateDeliveryOptionRequest>
{
    public UpdateDeliveryOptionValidator(IDeliveryOptionRepository deliveryOptionRepository)
    {
        RuleFor(x => new { x.Title, x.Id })
             .MustAsync(async (x, CancellationToken)
                    => !await deliveryOptionRepository.AnyAsync(c => c.Title == x.Title.Trim() && c.Id != x.Id))
             .WithMessage(MessageKeys.DuplicateTitleOrAddress)
             .When(x => !string.IsNullOrWhiteSpace(x.Title));

        RuleFor(x => x.DeliveryDays)
            .GreaterThan(0)
            .WithMessage(MessageKeys.MinDeliveryDays1);
    }
}