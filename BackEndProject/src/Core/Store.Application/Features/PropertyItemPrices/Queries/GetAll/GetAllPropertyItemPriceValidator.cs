using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public class GetAllPropertyItemPriceValidator : AbstractValidator<GetAllPropertyItemPriceRequest>
{
    public GetAllPropertyItemPriceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyCategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyItemId).MustBeValidOptionalEntityId();
    }
}
