using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Languages.Commands;

public class CreateLanguageValidator : AbstractValidator<CreateLanguageRequest>
{
    public CreateLanguageValidator(ILanguageRepository languageRepository)
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest)
            .MaximumLength(10)
            .WithMessage(MessageKeys.MaxLength10Characters)
            .MustAsync(async (code, cancellationToken) =>
            {
                var normalizedCode = LanguageCultureNormalizer.Normalize(code);
                return !await languageRepository.AnyAsync(x => x.Code == normalizedCode, cancellationToken);
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
