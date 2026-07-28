using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductProperties.Queries;

public class SearchProductPropertyValidator : AbstractValidator<SearchProductPropertyRequest>
{
    public SearchProductPropertyValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.ProductId).MustBeValidOptionalEntityId();
        RuleFor(x => x.PropertyId).MustBeValidOptionalEntityId();
    }
}
