using FluentValidation;
using Store.Common.Localization;

namespace Edition.Application.Features.Orders.Queries;

public class GetOrderStatusesValidator : AbstractValidator<GetOrderStatusesRequest>
{
    public GetOrderStatusesValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .WithMessage(MessageKeys.InvalidRequest);
    }
}
