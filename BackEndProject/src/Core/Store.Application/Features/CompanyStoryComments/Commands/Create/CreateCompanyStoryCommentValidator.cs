using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class CreateCompanyStoryCommentValidator : AbstractValidator<CreateCompanyStoryCommentRequest>
{
    public CreateCompanyStoryCommentValidator()
    {
        RuleFor(x => x.CompanyStoryId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidCompanyStoryId);
    }
}
