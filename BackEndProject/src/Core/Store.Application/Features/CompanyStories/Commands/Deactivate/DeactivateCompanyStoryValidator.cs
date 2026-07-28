using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStories.Commands;

public class DeactivateCompanyStoryValidator : AbstractValidator<DeactivateCompanyStoryRequest>
{
    public DeactivateCompanyStoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
