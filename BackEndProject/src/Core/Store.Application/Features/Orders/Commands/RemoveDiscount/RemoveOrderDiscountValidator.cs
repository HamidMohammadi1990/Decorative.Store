using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Commands;

public class RemoveOrderDiscountValidator : AbstractValidator<RemoveOrderDiscountRequest>
{
    public RemoveOrderDiscountValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
