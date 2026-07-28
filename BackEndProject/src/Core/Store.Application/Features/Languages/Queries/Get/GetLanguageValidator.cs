using Edition.Application.Common.Validation;
using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Languages.Queries;

public class GetLanguageValidator : AbstractValidator<GetLanguageRequest>
{
    public GetLanguageValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidEntityId()
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
