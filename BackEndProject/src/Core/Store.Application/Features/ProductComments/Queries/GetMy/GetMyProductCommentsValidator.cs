using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.ProductComments.Queries;

public class GetMyProductCommentsValidator : AbstractValidator<GetMyProductCommentsRequest>
{
    public GetMyProductCommentsValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
