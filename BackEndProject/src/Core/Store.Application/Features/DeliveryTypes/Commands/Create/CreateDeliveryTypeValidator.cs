using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class CreateDeliveryTypeValidator : AbstractValidator<CreateDeliveryTypeRequest>
{
    public CreateDeliveryTypeValidator(IDeliveryTypeRepository deliveryTypeRepository)
    {
        RuleFor(x => x.Title)            
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, CancellationToken)
                   => !await deliveryTypeRepository.AnyAsync(c => c.Title == x.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}