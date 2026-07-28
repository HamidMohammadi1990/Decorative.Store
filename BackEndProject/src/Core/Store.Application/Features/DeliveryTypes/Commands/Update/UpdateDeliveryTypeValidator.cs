using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class UpdateDeliveryTypeValidator : AbstractValidator<UpdateDeliveryTypeRequest>
{
    public UpdateDeliveryTypeValidator(IDeliveryTypeRepository deliveryTypeRepository)
    {
        RuleFor(x => new { x.Id, x.Title })
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MustAsync(async (x, CancellationToken)
                   => !await deliveryTypeRepository.AnyAsync(c => c.Id != x.Id && c.Title == x.Title.Trim()))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
