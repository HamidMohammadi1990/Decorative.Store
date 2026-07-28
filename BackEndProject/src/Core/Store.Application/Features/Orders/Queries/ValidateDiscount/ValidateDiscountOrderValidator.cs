using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class ValidateDiscountOrderValidator : AbstractValidator<ValidateDiscountOrderRequest>
{
    public ValidateDiscountOrderValidator()
    {
        RuleFor(x => x.DiscountCode)
            .NotEmpty()
            .WithMessage(MessageKeys.DiscountCodeRequired);
    }
}
