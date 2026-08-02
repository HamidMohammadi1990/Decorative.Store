using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Commands;

public class AddCartItemValidator : AbstractValidator<AddCartItemRequest>
{
    public AddCartItemValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
