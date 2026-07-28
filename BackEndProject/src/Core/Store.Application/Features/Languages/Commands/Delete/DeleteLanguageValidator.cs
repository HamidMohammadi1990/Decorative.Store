using Edition.Application.Common.Validation;
using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Languages.Commands;

public class DeleteLanguageValidator : AbstractValidator<DeleteLanguageRequest>
{
    public DeleteLanguageValidator()
    {
        RuleFor(x => x.Id)
            .MustBeValidEntityId()
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
