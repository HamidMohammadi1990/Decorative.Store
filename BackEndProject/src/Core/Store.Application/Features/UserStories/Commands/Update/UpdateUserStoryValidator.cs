using FluentValidation;

namespace Edition.Application.Features.UserStories.Commands;

public class UpdateUserStoryValidator : AbstractValidator<UpdateUserStoryRequest>
{
    public UpdateUserStoryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(70);
        RuleFor(x => x.Caption).MaximumLength(500);
    }
}
