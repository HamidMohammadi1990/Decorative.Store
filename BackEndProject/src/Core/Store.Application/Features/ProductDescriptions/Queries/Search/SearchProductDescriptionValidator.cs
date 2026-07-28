using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public class SearchProductDescriptionValidator : AbstractValidator<SearchProductDescriptionRequest>
{
    public SearchProductDescriptionValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidEntityId();
    }
}
