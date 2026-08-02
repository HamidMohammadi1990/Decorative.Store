using FluentValidation;
using Edition.Application.Common.Validation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyItems.Commands;

public class UpdatePropertyItemValidator : AbstractValidator<UpdatePropertyItemRequest>
{
    public UpdatePropertyItemValidator(IPropertyItemRepository propertyItemRepository, ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
        RuleFor(x => x.PropertyId).MustBeValidEntityId();

        RuleFor(x => x.LanguageId)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidRequest)
            .MustAsync(async (languageId, cancellationToken) =>
                await languageRepository.FindAsync(languageId, cancellationToken) is { IsActive: true })
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(MessageKeys.TitleRequired)
            .MaximumLength(EntityFieldLengths.PropertyItem.Title)
            .WithMessage(MessageKeys.MaxLength30Characters);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest);

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                !await propertyItemRepository.ExistsCodeAsync(
                    request.Code,
                    request.PropertyId,
                    request.Id,
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
