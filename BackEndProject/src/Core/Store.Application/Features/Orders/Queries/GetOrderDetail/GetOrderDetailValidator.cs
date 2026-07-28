using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderDetailValidator : AbstractValidator<GetOrderDetailRequest>
{
    public GetOrderDetailValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEqual(0)
            .WithMessage(MessageKeys.InvalidId);
    }
}
