using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class CreateBlogPostCommentValidator : AbstractValidator<CreateBlogPostCommentRequest>
{
    public CreateBlogPostCommentValidator()
    {
        RuleFor(x => x.BlogPostId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidBlogPostId);
    }
}
