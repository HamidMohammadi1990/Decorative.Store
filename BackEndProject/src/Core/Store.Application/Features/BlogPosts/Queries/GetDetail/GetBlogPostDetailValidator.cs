using FluentValidation;

namespace Edition.Application.Features.BlogPosts.Queries;

public class GetBlogPostDetailValidator : AbstractValidator<GetBlogPostDetailRequest>
{
    public GetBlogPostDetailValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(200);
    }
}
