using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;
namespace Edition.Application.Features.Discounts.Commands;
public class CreateDiscountValidator : AbstractValidator<CreateDiscountRequest>
{
    public CreateDiscountValidator(IDiscountRepository discountRepository)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.DiscountCodeRequired)
            .MustAsync(async (code, cancellationToken)
                => !await discountRepository.AnyAsync(x => x.Code == code.Trim()))
            .WithMessage(MessageKeys.DuplicateDiscountCode);
    }
}