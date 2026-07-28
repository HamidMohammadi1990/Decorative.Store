using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class GetProductOrderItemAttachmentTypeValidator : AbstractValidator<GetProductOrderItemAttachmentTypeRequest>
{
    public GetProductOrderItemAttachmentTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
