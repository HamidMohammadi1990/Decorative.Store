using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Commands;

public class UpdatePageValidator : AbstractValidator<UpdatePageRequest>
{
    public UpdatePageValidator(IPageRepository pageRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage(MessageKeys.SlugRequired);

        RuleFor(x => new { x.Id, x.Slug })
            .MustAsync(async (x, cancellationToken)
                => !await pageRepository.AnyAsync(b => b.Id != x.Id && b.Slug == x.Slug.Trim()))
            .WithMessage(MessageKeys.DuplicateSlug);
    }
}
