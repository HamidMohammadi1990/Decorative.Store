using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public class GetProductPropertyPriceValidator : AbstractValidator<GetProductPropertyPriceRequest>
{
    public GetProductPropertyPriceValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
