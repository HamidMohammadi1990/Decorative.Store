using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.PostTypes.Queries;

public class SearchPostTypeValidator : AbstractValidator<SearchPostTypeRequest>
{
    public SearchPostTypeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.PostType.Title);
    }
}
