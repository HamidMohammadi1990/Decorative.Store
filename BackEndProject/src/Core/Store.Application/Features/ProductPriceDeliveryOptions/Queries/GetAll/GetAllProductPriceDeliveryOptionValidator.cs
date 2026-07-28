using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public class GetAllProductPriceDeliveryOptionValidator : AbstractValidator<GetAllProductPriceDeliveryOptionRequest>
{
    public GetAllProductPriceDeliveryOptionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CompanyId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ProductPriceId).MustBeValidOptionalEntityId();
        RuleFor(x => x.DeliveryOptionId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
    }
}
