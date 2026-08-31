using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.UserStoryLikes.Commands;

public class ToggleUserStoryLikeValidator : AbstractValidator<ToggleUserStoryLikeRequest>
{
    public ToggleUserStoryLikeValidator()
    {
        RuleFor(x => x.UserStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidUserStoryId);
    }
}
