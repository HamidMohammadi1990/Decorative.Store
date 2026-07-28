using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPosts.Queries;

public class SearchBlogPostValidator : AbstractValidator<SearchBlogPostRequest>
{
    public SearchBlogPostValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPost.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPost.Slug);
    }
}
