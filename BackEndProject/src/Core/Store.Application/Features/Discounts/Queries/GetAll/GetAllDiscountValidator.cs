using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Discounts.Queries;

public class GetAllDiscountValidator : AbstractValidator<GetAllDiscountRequest>
{
    public GetAllDiscountValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Code).MaximumLengthWhenNotEmpty(EntityFieldLengths.Discount.Code);
    }
}
