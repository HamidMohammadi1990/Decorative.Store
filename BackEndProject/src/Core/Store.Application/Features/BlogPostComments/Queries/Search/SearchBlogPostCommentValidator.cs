using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class SearchBlogPostCommentValidator : AbstractValidator<SearchBlogPostCommentRequest>
{
    public SearchBlogPostCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.BlogPostId).MustBeValidEntityId();
    }
}
