using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryTypes.Queries;

public class GetDeliveryTypeValidator : AbstractValidator<GetDeliveryTypeRequest>
{
    public GetDeliveryTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
