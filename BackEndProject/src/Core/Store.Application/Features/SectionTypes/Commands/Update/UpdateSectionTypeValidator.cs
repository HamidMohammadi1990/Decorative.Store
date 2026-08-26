using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Commands;

public class UpdateSectionTypeValidator : AbstractValidator<UpdateSectionTypeRequest>
{
    public UpdateSectionTypeValidator(
        ISectionTypeRepository repository,
        ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await repository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Name,
                    request.Id,
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
