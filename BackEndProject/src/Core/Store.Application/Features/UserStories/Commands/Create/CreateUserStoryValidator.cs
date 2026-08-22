using Edition.Application.Common.Directories;
using FluentValidation;

namespace Edition.Application.Features.UserStories.Commands;

public class CreateUserStoryValidator : AbstractValidator<CreateUserStoryRequest>
{
    public CreateUserStoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(70);

        RuleFor(x => x.Caption)
            .MaximumLength(500);

        RuleFor(x => x.MediaPath)
            .NotEmpty()
            .MaximumLength(260)
            .Must(path => path.StartsWith(UserStoryDirectory.PublicMediaPrefix, StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.MediaAlt)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(x => x.PosterPath)
            .MaximumLength(260)
            .Must(path => path is null || path.StartsWith(UserStoryDirectory.PublicMediaPrefix, StringComparison.OrdinalIgnoreCase))
            .When(x => !string.IsNullOrWhiteSpace(x.PosterPath));

        RuleFor(x => x.MediaType)
            .IsInEnum();
    }
}
