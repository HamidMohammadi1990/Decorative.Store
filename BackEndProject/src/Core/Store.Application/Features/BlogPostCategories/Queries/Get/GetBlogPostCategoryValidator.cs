using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostCategories.Queries;

public class GetBlogPostCategoryValidator : AbstractValidator<GetBlogPostCategoryRequest>
{
    public GetBlogPostCategoryValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
