using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Languages.Queries;

public class GetAllLanguageValidator : AbstractValidator<GetAllLanguageRequest>
{
    public GetAllLanguageValidator()
    {
        RuleFor(x => x.Pagination)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
