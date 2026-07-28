using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class GetStatusSummaryOrderValidator : AbstractValidator<GetStatusSummaryOrderRequest>
{
    public GetStatusSummaryOrderValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
