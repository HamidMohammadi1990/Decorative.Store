using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.BlogPostCategories.Commands;

public class UpdateBlogPostCategoryValidator : AbstractValidator<UpdateBlogPostCategoryRequest>
{
    public UpdateBlogPostCategoryValidator(IBlogPostCategoryRepository blogPostCategoryRepository)
    {
        RuleFor(x => new { x.Id, x.Title, x.Slug })
          .NotNull()
          .WithMessage(MessageKeys.TitleRequired)
          .MustAsync(async (x, CancellationToken)
                 => !await blogPostCategoryRepository.AnyAsync(c => c.Id != x.Id && c.Title == x.Title.Trim() || c.Slug == x.Slug.Trim()))
          .WithMessage(MessageKeys.DuplicateTitle);
    }
}
