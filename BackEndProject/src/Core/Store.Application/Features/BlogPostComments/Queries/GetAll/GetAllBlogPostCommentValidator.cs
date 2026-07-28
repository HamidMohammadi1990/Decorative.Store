using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class GetAllBlogPostCommentValidator : AbstractValidator<GetAllBlogPostCommentRequest>
{
    public GetAllBlogPostCommentValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.BlogPostId).MustBeValidOptionalEntityId();
        RuleFor(x => x.CreatedByUserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.ApprovedByUserId).MustBeValidOptionalEntityId();
    }
}
