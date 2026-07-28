using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.DeliveryTypes.Commands;

public class DeleteDeliveryTypeValidator : AbstractValidator<DeleteDeliveryTypeRequest>
{
    public DeleteDeliveryTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
