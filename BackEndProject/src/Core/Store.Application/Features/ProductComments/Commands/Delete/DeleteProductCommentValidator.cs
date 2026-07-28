using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Commands;

public class DeleteProductCommentValidator : AbstractValidator<DeleteProductCommentRequest>
{
    public DeleteProductCommentValidator()
    {
        RuleFor(x => x.Id)
          .Equal(0)
          .WithMessage(MessageKeys.InvalidId);
    }
}