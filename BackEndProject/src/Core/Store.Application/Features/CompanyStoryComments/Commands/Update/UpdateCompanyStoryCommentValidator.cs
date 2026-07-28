using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class UpdateCompanyStoryCommentValidator : AbstractValidator<UpdateCompanyStoryCommentRequest>
{
    public UpdateCompanyStoryCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
