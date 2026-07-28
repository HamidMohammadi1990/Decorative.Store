using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.CommentTopics.Queries;

public class GetAllCommentTopicValidator : AbstractValidator<GetAllCommentTopicRequest>
{
    public GetAllCommentTopicValidator()
    {
        RuleFor(x => x.Pagination).MustBeValidPagination();
        RuleFor(x => x.Title).MaximumLengthWhenNotEmpty(EntityFieldLengths.CommentTopic.Title);
    }
}
