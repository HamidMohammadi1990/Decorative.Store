using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostTags.Queries;

public class GetBlogPostTagValidator : AbstractValidator<GetBlogPostTagRequest>
{
    public GetBlogPostTagValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
