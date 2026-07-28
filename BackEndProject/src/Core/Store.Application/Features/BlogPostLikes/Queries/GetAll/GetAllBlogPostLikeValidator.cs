using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public class GetAllBlogPostLikeValidator : AbstractValidator<GetAllBlogPostLikeRequest>
{
    public GetAllBlogPostLikeValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.BlogPostId).MustBeValidOptionalEntityId();
    }
}
