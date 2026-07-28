using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Commands;

public class ChangeStatusProductCommentValidator : AbstractValidator<ChangeStatusProductCommentRequest>
{
    public ChangeStatusProductCommentValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}
