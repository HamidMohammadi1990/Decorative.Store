using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Commands;

public class RemoveOrderItemValidator : AbstractValidator<RemoveOrderItemRequest>
{
    public RemoveOrderItemValidator()
    {
        RuleFor(x => x.OrderItemId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}
