using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class UpdateBlogPostTagValidator : AbstractValidator<UpdateBlogPostTagRequest>
{
    public UpdateBlogPostTagValidator(IBlogPostTagRepository blogPostTagRepository)
    {
        RuleFor(x => new { x.Id, x.TagId, x.BlogPostId })
         .NotNull()
         .WithMessage(MessageKeys.TitleRequired)
         .MustAsync(async (x, CancellationToken)
                => !await blogPostTagRepository.AnyAsync(c => c.Id != x.Id && c.TagId == x.TagId && c.BlogPostId == x.BlogPostId))
         .WithMessage(MessageKeys.DuplicateTag);
    }
}
