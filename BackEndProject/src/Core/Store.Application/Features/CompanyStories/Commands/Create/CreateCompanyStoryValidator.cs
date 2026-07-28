using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStories.Commands;

public class CreateCompanyStoryValidator : AbstractValidator<CreateCompanyStoryRequest>
{
    public CreateCompanyStoryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
