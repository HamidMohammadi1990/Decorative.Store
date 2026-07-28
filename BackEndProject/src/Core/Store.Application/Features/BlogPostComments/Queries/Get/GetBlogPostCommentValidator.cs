using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostComments.Queries;

public class GetBlogPostCommentValidator : AbstractValidator<GetBlogPostCommentRequest>
{
    public GetBlogPostCommentValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
