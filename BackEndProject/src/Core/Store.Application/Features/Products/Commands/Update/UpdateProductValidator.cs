using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Products.Commands;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator(IProductRepository productRepository, ILanguageRepository languageRepository)
    {
        RuleFor(u => u.Id)
           .NotEqual(0)
           .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotNull()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.TitleMaxLength50)
            .MinimumLength(10)
            .WithMessage(MessageKeys.TitleMinLength10);

        RuleFor(x => x.Status)
            .NotNull()
            .WithMessage(MessageKeys.StatusTypeRequired);

        RuleFor(x => x.ProductCode)
            .NotNull()
            .WithMessage(MessageKeys.ProductCodeRequired);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await productRepository.ExistsProductCodeAsync(request.ProductCode, request.Id, cancellationToken))
            .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.ProductCode));

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await productRepository.ExistsTranslationAsync(
                    request.LanguageId,
                    request.Title,
                    request.Slug,
                    request.Id,
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateProduct)
            .When(x => !string.IsNullOrEmpty(x.Title));
    }
}
