using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class CreateProductOrderItemAttachmentTypeHandler
    (IUnitOfWork uow, IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    : IRequestHandler<CreateProductOrderItemAttachmentTypeRequest, OperationResult<CreateProductOrderItemAttachmentTypeResponse>>
{
    public async Task<OperationResult<CreateProductOrderItemAttachmentTypeResponse>> Handle(CreateProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var productOrderItemAttachmentType = ProductOrderItemAttachmentType.Create(
            request.ProductId,
            request.Description,
            request.OrderItemAttachmentTypeId,
            request.Priority);

        productOrderItemAttachmentTypeRepository.Add(productOrderItemAttachmentType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult.ToGenericFailure<CreateProductOrderItemAttachmentTypeResponse>();

        return new CreateProductOrderItemAttachmentTypeResponse { Id = productOrderItemAttachmentType.Id };
    }
}
