using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class SearchBlogPostCategoryValidator : AbstractValidator<SearchBlogPostCategoryRequest>
{
    public SearchBlogPostCategoryValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Code).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPostCategory.Code);
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPostCategory.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPostCategory.Slug);
    }
}
