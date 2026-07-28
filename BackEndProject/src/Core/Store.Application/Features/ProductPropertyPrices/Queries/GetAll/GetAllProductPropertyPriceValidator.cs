using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public class GetAllProductPropertyPriceValidator : AbstractValidator<GetAllProductPropertyPriceRequest>
{
    public GetAllProductPropertyPriceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ProductPropertyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
    }
}
