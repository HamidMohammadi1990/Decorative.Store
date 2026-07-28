using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStories.Commands;

public class UpdateCompanyStoryValidator : AbstractValidator<UpdateCompanyStoryRequest>
{
    public UpdateCompanyStoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
