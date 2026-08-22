using FluentValidation;

namespace Edition.Application.Features.UserStories.Commands;

public class DeleteUserStoryValidator : AbstractValidator<DeleteUserStoryRequest>
{
    public DeleteUserStoryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
