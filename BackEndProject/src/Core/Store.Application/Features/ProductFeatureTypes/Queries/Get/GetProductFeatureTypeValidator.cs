using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public class GetProductFeatureTypeValidator : AbstractValidator<GetProductFeatureTypeRequest>
{
    public GetProductFeatureTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
