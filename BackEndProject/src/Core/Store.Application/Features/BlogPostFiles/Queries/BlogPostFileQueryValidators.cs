using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostFiles.Queries;

public class GetAllBlogPostFileValidator : AbstractValidator<GetAllBlogPostFileRequest>
{
    public GetAllBlogPostFileValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.BlogPostId).MustBeValidOptionalEntityId();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.BlogPostFile.Title);
    }
}

public class GetBlogPostFileValidator : AbstractValidator<GetBlogPostFileRequest>
{
    public GetBlogPostFileValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
