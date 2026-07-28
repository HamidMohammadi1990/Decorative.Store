using FluentValidation;
using Store.Common.Localization;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class CreateProductOrderItemAttachmentTypeValidator : AbstractValidator<CreateProductOrderItemAttachmentTypeRequest>
{
    public CreateProductOrderItemAttachmentTypeValidator(IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    {
        RuleFor(x => new { x.ProductId, x.OrderItemAttachmentTypeId })
            .MustAsync(async (x, cancellationToken)
                => !await productOrderItemAttachmentTypeRepository.AnyAsync(c =>
                    c.ProductId == x.ProductId &&
                    c.OrderItemAttachmentTypeId == x.OrderItemAttachmentTypeId))
            .WithMessage(MessageKeys.DuplicateProductAttachmentType);
    }
}
