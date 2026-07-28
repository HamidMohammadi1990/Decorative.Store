using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class ApproveCompanyStoryCommentValidator : AbstractValidator<ApproveCompanyStoryCommentRequest>
{
    public ApproveCompanyStoryCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
