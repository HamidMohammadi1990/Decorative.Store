using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStories.Queries;

public class GetCompanyStoryValidator : AbstractValidator<GetCompanyStoryRequest>
{
    public GetCompanyStoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
