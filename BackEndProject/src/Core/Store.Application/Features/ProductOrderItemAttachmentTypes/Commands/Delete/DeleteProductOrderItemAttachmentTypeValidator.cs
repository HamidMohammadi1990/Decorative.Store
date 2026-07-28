using FluentValidation;
using Edition.Application.Common.Validation;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class DeleteProductOrderItemAttachmentTypeValidator : AbstractValidator<DeleteProductOrderItemAttachmentTypeRequest>
{
    public DeleteProductOrderItemAttachmentTypeValidator()
    {
        RuleFor(x => x.Id).MustBeValidEntityId();
    }
}
