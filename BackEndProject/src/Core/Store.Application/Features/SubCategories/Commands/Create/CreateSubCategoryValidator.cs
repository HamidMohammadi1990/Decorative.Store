using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Commands;

public class CreateSubCategoryValidator : AbstractValidator<CreateSubCategoryRequest>
{
    public CreateSubCategoryValidator(ISubCategoryRepository subCategoryRepository, ILanguageRepository languageRepository)
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

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await subCategoryRepository.ExistsCodeAsync(request.Code, cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle)
            .When(x => !string.IsNullOrEmpty(x.Code));

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await subCategoryRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}
