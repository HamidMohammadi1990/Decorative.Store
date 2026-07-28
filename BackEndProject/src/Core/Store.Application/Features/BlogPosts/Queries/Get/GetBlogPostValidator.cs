using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetBlogPostValidator : AbstractValidator<GetBlogPostRequest>
{
    public GetBlogPostValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
