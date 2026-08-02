using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator(IProductRepository productRepository, ILanguageRepository languageRepository)
    {
        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired);

        RuleFor(x => x.ProductCode)
            .NotNull()
            .WithMessage(MessageKeys.ProductCodeRequired);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await productRepository.ExistsProductCodeAsync(request.ProductCode, cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.ProductCode));

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await productRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}
