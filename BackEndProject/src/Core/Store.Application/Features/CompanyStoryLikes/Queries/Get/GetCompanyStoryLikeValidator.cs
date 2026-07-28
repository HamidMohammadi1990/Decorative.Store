using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public class GetCompanyStoryLikeValidator : AbstractValidator<GetCompanyStoryLikeRequest>
{
    public GetCompanyStoryLikeValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
