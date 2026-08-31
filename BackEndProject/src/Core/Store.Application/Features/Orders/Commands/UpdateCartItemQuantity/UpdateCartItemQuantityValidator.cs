using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Commands;

public class UpdateCartItemQuantityValidator : AbstractValidator<UpdateCartItemQuantityRequest>
{
    public UpdateCartItemQuantityValidator()
    {
        RuleFor(x => x.OrderItemId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
