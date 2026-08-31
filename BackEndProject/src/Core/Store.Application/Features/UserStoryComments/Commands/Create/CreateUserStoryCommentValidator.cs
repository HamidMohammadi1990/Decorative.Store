using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;

namespace Edition.Application.Features.UserStoryComments.Commands;

public class CreateUserStoryCommentValidator : AbstractValidator<CreateUserStoryCommentRequest>
{
    public CreateUserStoryCommentValidator()
    {
        RuleFor(x => x.UserStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidUserStoryId);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(EntityFieldLengths.UserStoryComment.Content);
    }
}
