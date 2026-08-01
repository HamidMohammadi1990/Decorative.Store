using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Commands;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryValidator(ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await categoryRepository.ExistsCodeAsync(request.Code, cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await categoryRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
