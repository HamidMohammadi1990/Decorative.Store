using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPosts.Commands;

public class UpdateBlogPostValidator : AbstractValidator<UpdateBlogPostRequest>
{
    public UpdateBlogPostValidator(IBlogPostRepository blogPostRepository)
    {
        RuleFor(x => new { x.Title, x.Slug, x.Id, x.CategoryId })
           .NotNull()
           .WithMessage(MessageKeys.TitleRequired)
           .MustAsync(async (x, CancellationToken)
                  => !await blogPostRepository.AnyAsync(c => c.Id != x.Id && c.BlogPostCategoryId == x.CategoryId && c.Title == x.Title.Trim() || c.Slug == x.Slug.Trim()))
           .WithMessage(MessageKeys.DuplicateTitle);
    }
}
