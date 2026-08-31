using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.UserStoryComments.Queries;

public class SearchUserStoryCommentValidator : AbstractValidator<SearchUserStoryCommentRequest>
{
    public SearchUserStoryCommentValidator()
    {
        RuleFor(x => x.UserStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidUserStoryId);

        RuleFor(x => x.Pagination).NotNull();
    }
}
