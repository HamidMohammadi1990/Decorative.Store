using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Discounts.Commands;

public class DeleteDiscountValidator : AbstractValidator<DeleteDiscountRequest>
{
    public DeleteDiscountValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
