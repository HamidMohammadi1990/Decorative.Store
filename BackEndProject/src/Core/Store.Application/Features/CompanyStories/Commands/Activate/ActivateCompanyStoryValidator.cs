using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStories.Commands;

public class ActivateCompanyStoryValidator : AbstractValidator<ActivateCompanyStoryRequest>
{
    public ActivateCompanyStoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
