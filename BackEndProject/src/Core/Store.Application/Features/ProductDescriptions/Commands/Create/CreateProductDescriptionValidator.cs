using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public class CreateProductDescriptionValidator : AbstractValidator<CreateProductDescriptionRequest>
{
    public CreateProductDescriptionValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x.ProductId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidProductId);

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Description)
            .NotNull()
            .WithMessage(MessageKeys.DescriptionRequired);
    }
}