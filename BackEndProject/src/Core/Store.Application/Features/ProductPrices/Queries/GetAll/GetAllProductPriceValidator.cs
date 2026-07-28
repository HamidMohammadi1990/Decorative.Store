using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPrices.Queries;

public class GetAllProductPriceValidator : AbstractValidator<GetAllProductPriceRequest>
{
    public GetAllProductPriceValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
    }
}
