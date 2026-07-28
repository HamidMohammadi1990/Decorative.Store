using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class DeleteBlogPostCommentValidator : AbstractValidator<DeleteBlogPostCommentRequest>
{
    public DeleteBlogPostCommentValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
