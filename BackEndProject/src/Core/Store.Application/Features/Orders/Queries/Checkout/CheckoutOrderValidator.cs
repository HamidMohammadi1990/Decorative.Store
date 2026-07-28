using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class CheckoutOrderValidator : AbstractValidator<CheckoutOrderRequest>
{
    public CheckoutOrderValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId);
    }
}