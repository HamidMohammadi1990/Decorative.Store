using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.BlogPostTags.Commands;

public class DeleteBlogPostTagValidator : AbstractValidator<DeleteBlogPostTagRequest>
{
    public DeleteBlogPostTagValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
