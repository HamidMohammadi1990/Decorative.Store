using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Commands;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator(ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
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
                !await categoryRepository.ExistsCodeAsync(request.Code, request.Id, cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await categoryRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    request.Id,
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
