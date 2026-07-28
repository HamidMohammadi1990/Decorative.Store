using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryOptions.Queries;

public class GetDeliveryOptionValidator : AbstractValidator<GetDeliveryOptionRequest>
{
    public GetDeliveryOptionValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
