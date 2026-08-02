using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Commands;

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyRequest>
{
    public UpdatePropertyValidator(IPropertyRepository propertyRepository, ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(MessageKeys.InvalidId)
            .MustAsync(async (id, cancellationToken) =>
                await propertyRepository.FindAsync(id, cancellationToken) is not null)
            .WithMessage(MessageKeys.InvalidId);

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
                !await propertyRepository.ExistsCodeAsync(
                    request.Code,
                    request.PropertyCategoryId,
                    request.Id,
                    cancellationToken))
            .WithMessage(MessageKeys.DuplicateTitle);
    }
}
