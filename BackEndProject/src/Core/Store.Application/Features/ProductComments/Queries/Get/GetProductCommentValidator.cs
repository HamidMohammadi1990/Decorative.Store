using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetProductCommentValidator : AbstractValidator<GetProductCommentRequest>
{
    public GetProductCommentValidator()
    {
        RuleFor(x => x.Id)
            .Equal(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}