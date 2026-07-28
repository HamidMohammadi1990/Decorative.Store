using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Categories.Queries;

public class SearchCategoryValidator : AbstractValidator<SearchCategoryRequest>
{
    public SearchCategoryValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Slug);
        RuleFor(x => x.Code).MaximumLengthWhenNotEmpty(EntityFieldLengths.Category.Code);
    }
}
