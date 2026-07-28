using Edition.Application.Common.Validation;
using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Languages.Queries;

public class SearchLanguageValidator : AbstractValidator<SearchLanguageRequest>
{
    public SearchLanguageValidator()
    {
        RuleFor(x => x.Pagination)
            .MustBeValidPagination();

        RuleFor(x => x.Code)
            .MaximumLength(10)
            .WithMessage(MessageKeys.MaxLength10Characters)
            .When(x => !string.IsNullOrWhiteSpace(x.Code));

        RuleFor(x => x.Name)
            .MaximumLength(50)
            .WithMessage(MessageKeys.MaxLength50Characters)
            .When(x => !string.IsNullOrWhiteSpace(x.Name));
    }
}
