using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public class GetCompanyStoryCommentValidator : AbstractValidator<GetCompanyStoryCommentRequest>
{
    public GetCompanyStoryCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
