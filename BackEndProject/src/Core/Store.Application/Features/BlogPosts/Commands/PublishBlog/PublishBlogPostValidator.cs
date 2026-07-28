using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.BlogPosts.Commands;

public class PublishBlogPostValidator : AbstractValidator<PublishBlogPostRequest>
{
    public PublishBlogPostValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidBlogPostId);
    }
}
