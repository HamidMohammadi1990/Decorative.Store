using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Commands;

public class PurchaseOrderValidator : AbstractValidator<PurchaseOrderRequest>
{
    public PurchaseOrderValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(EntityFieldLengths.Order.Title)
            .WithMessage(MessageKeys.TitleMaxLength50);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId);

        RuleFor(x => x.UserAddressId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
