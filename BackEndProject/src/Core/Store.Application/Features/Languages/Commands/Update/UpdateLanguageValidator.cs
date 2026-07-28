using Edition.Application.Common.Validation;
using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageRequest>
{
    public UpdateLanguageValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Id)
            .MustBeValidEntityId()
            .WithMessage(MessageKeys.InvalidIdValidator);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(10)
            .WithMessage(MessageKeys.MaxLength10Characters)
            .MustAsync(async (request, code, cancellationToken) =>
            {
                var normalizedCode = LanguageCultureNormalizer.Normalize(code);
                return !await languageRepository.AnyAsync(
                    x => x.Code == normalizedCode && x.Id != request.Id,
                    cancellationToken);
            })
            .WithMessage(MessageKeys.DuplicateRecord);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(MessageKeys.NameRequired)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters);

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0)
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
