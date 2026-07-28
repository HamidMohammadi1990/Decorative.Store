using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CommentTopics.Queries;

public class GetCommentTopicValidator : AbstractValidator<GetCommentTopicRequest>
{
    public GetCommentTopicValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
