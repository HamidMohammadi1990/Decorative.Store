using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class CreatePropertyItemValidator : AbstractValidator<CreatePropertyItemRequest>
{
    public CreatePropertyItemValidator(IPropertyItemRepository propertyItemRepository, ILanguageRepository languageRepository)
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
                !await propertyItemRepository.ExistsCodeAsync(
                    request.Code,
                    request.PropertyId,
                    cancellationToken: cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
