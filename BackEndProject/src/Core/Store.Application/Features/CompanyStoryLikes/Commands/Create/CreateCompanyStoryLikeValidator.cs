using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryLikes.Commands;

public class CreateCompanyStoryLikeValidator : AbstractValidator<CreateCompanyStoryLikeRequest>
{
    public CreateCompanyStoryLikeValidator()
    {
        RuleFor(x => x.CompanyStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidCompanyStoryId);
    }
}
