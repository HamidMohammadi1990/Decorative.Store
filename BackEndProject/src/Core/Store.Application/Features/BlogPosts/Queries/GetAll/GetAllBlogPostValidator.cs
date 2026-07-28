using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetAllBlogPostValidator : AbstractValidator<GetAllBlogPostRequest>
{
    public GetAllBlogPostValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.CategoryId).MustBeValidOptionalEntityId();
        RuleFor(x => x.UserId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPost.Title);
        RuleFor(x => x.Slug).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPost.Slug);
    }
}
