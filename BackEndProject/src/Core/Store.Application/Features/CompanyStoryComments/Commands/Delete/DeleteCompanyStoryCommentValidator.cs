using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyStoryComments.Commands;

public class DeleteCompanyStoryCommentValidator : AbstractValidator<DeleteCompanyStoryCommentRequest>
{
    public DeleteCompanyStoryCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidIdValidator);
    }
}
