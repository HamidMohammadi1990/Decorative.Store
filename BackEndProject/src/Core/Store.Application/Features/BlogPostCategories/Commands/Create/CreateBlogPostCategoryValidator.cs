using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public class CreateBlogPostCategoryValidator : AbstractValidator<BlogPostCategory>
{
    public CreateBlogPostCategoryValidator(IBlogPostCategoryRepository blogPostCategoryRepository)
    {
        RuleFor(x => new { x.Title, x.Slug })
           .NotNull()
           .WithMessage(MessageKeys.TitleRequired)
           .MustAsync(async (x, CancellationToken)
                  => !await blogPostCategoryRepository.AnyAsync(c => c.Title == x.Title.Trim() || c.Slug == x.Slug.Trim()))
           .WithMessage(MessageKeys.DuplicateTitle);
    }
}
