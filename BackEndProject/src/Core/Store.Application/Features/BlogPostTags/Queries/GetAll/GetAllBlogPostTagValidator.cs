using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostTags.Queries;

public class GetAllBlogPostTagValidator : AbstractValidator<GetAllBlogPostTagRequest>
{
    public GetAllBlogPostTagValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.BlogPostId).MustBeValidOptionalEntityId();
        RuleFor(x => x.TagId).MustBeValidOptionalEntityId();
        RuleFor(x => x.TagTitle).MaximumLengthWhenNotEmpty(EntityFieldLengths.Tag.Title);
    }
}
