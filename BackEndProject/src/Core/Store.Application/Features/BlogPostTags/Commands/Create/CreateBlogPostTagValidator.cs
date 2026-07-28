using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class CreateBlogPostTagValidator : AbstractValidator<CreateBlogPostTagRequest>
{
    public CreateBlogPostTagValidator(IBlogPostTagRepository blogPostTagRepository)
    {
        RuleFor(x => new { x.TagId, x.BlogPostId })
          .NotNull()
          .WithMessage(MessageKeys.TitleRequired)
          .MustAsync(async (x, CancellationToken)
                 => !await blogPostTagRepository.AnyAsync(c => c.TagId == x.TagId && c.BlogPostId == x.BlogPostId))
          .WithMessage(MessageKeys.DuplicateTag);
    }
}
