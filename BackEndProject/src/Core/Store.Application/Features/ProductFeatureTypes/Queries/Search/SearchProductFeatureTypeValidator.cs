using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductFeatureTypes.Queries;

public class SearchProductFeatureTypeValidator : AbstractValidator<SearchProductFeatureTypeRequest>
{
    public SearchProductFeatureTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Name).MaximumLengthWhenNotEmpty(EntityFieldLengths.ProductFeatureType.Name);
    }
}
