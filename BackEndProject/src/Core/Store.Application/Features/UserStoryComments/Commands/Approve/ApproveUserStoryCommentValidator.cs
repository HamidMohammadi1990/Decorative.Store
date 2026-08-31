using FluentValidation;

namespace Edition.Application.Features.UserStoryComments.Commands;

public class ApproveUserStoryCommentValidator : AbstractValidator<ApproveUserStoryCommentRequest>
{
    public ApproveUserStoryCommentValidator()
    {
        RuleFor(x => x.Id).NotEqual(0);
    }
}
