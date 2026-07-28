using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.BlogPostComments.Commands;

public class ApproveBlogPostCommentValidator : AbstractValidator<ApproveBlogPostCommentRequest>
{
    public ApproveBlogPostCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}
