using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Pages.Commands;

public class CreatePageValidator : AbstractValidator<CreatePageRequest>
{
    public CreatePageValidator(IPageRepository pageRepository)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage(MessageKeys.SlugRequired)
            .MustAsync(async (slug, cancellationToken)
                => !await pageRepository.AnyAsync(x => x.Slug == slug.Trim()))
            .WithMessage(MessageKeys.DuplicateSlug);
    }
}
