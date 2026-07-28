using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class UpdateBlogPostCommentValidator : AbstractValidator<UpdateBlogPostCommentRequest>
{
    public UpdateBlogPostCommentValidator()
    {
        RuleFor(x => x.BlogPostId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidBlogPostId);
    }
}
