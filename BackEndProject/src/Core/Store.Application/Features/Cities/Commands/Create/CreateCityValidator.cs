using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Cities.Commands;

public class CreateCityValidator : AbstractValidator<CreateCityRequest>
{
    public CreateCityValidator(ICityRepository cityRepository)
    {
        RuleFor(x => new { x.Name, x.Slug })
             .MustAsync(async (x, CancellationToken)
                    => !await cityRepository.AnyAsync(c => c.Name == x.Name.Trim() || x.Slug == x.Slug.Trim()))
             .WithMessage(MessageKeys.DuplicateTitleOrAddress)
             .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Name)
            .MaximumLength(25)
            .WithMessage(MessageKeys.MaxLength25Characters);

        RuleFor(x => x.Name)
            .MinimumLength(5)
            .WithMessage(MessageKeys.MinLength5Characters);

        RuleFor(x => x.Slug)
            .MaximumLength(30)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.Description)
            .MaximumLength(200)
            .WithMessage(MessageKeys.MaxLength200Characters);
    }
}
