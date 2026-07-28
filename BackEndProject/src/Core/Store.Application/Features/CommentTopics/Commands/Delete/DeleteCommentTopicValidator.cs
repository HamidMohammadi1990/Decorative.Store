using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CommentTopics.Commands;

public class DeleteCommentTopicValidator : AbstractValidator<DeleteCommentTopicRequest>
{
    public DeleteCommentTopicValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
