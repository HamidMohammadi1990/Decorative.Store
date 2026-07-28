using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.CompanyComments.Commands;

public class ChangeStatusCompanyCommentValidator : AbstractValidator<ChangeStatusCompanyCommentRequest>
{
    public ChangeStatusCompanyCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
