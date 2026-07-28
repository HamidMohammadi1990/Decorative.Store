using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderValidator : AbstractValidator<GetOrderRequest>
{
    public GetOrderValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
