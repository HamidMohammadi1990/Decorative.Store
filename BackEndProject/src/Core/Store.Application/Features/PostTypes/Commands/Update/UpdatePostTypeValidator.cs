using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PostTypes.Commands;

public class UpdatePostTypeValidator : AbstractValidator<UpdatePostTypeRequest>
{
    public UpdatePostTypeValidator(IPostTypeRepository postTypeRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(EntityFieldLengths.PostType.Title)
            .WithMessage(MessageKeys.MaxLength35Characters);

        RuleFor(x => x.Description)
            .MaximumLength(EntityFieldLengths.PostType.Description)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage(MessageKeys.MaxLength150Characters);

        RuleFor(x => new { x.Id, x.Title })
            .MustAsync(async (x, _) =>
                !await postTypeRepository.AnyAsync(p => p.Id != x.Id && p.Title == x.Title.Trim()))
            .When(x => !string.IsNullOrWhiteSpace(x.Title))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
