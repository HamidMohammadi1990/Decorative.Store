using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;
namespace Edition.Application.Features.Discounts.Commands;
public class UpdateDiscountValidator : AbstractValidator<UpdateDiscountRequest>
{
    public UpdateDiscountValidator(IDiscountRepository discountRepository)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.DiscountCodeRequired);
        RuleFor(x => new { x.Id, x.Code })
            .MustAsync(async (x, cancellationToken)
                => !await discountRepository.AnyAsync(c => c.Id != x.Id && c.Code == x.Code.Trim()))
            .WithMessage(MessageKeys.DuplicateDiscountCode);
    }
}