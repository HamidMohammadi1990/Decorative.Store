using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostLikes.Commands;

public class CreateBlogPostLikeValidator : AbstractValidator<CreateBlogPostLikeRequest>
{
    public CreateBlogPostLikeValidator()
    {
        RuleFor(x => x.BlogPostId).MustBeValidEntityId();
    }
}
