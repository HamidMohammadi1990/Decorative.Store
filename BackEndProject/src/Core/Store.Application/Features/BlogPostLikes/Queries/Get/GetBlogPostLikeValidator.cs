using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostLikes.Queries;

public class GetBlogPostLikeValidator : AbstractValidator<GetBlogPostLikeRequest>
{
    public GetBlogPostLikeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
