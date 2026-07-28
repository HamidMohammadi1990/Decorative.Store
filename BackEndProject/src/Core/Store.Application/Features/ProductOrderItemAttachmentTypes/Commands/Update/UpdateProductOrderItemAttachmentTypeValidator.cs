using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class UpdateProductOrderItemAttachmentTypeValidator : AbstractValidator<UpdateProductOrderItemAttachmentTypeRequest>
{
    public UpdateProductOrderItemAttachmentTypeValidator(IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    {
        RuleFor(x => new { x.Id, x.ProductId, x.OrderItemAttachmentTypeId })
            .MustAsync(async (x, cancellationToken)
                => !await productOrderItemAttachmentTypeRepository.AnyAsync(c =>
                    c.Id != x.Id &&
                    c.ProductId == x.ProductId &&
                    c.OrderItemAttachmentTypeId == x.OrderItemAttachmentTypeId))
            .WithMessage(MessageKeys.DuplicateProductAttachmentType);
    }
}
