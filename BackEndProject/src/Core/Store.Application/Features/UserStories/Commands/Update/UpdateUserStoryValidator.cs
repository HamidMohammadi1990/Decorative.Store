using FluentValidation;

namespace Edition.Application.Features.UserStories.Commands;

public class UpdateUserStoryValidator : AbstractValidator<UpdateUserStoryRequest>
{
    public UpdateUserStoryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(70);
        RuleFor(x => x.Caption).MaximumLength(500);

        RuleFor(x => x.MediaPath)
            .MaximumLength(260)
            .When(x => !string.IsNullOrWhiteSpace(x.MediaPath));

        RuleFor(x => x.MediaAlt)
            .MaximumLength(120)
            .When(x => !string.IsNullOrWhiteSpace(x.MediaAlt));

        RuleFor(x => x.MediaType)
            .NotNull()
            .When(x => !string.IsNullOrWhiteSpace(x.MediaPath));
    }
}
