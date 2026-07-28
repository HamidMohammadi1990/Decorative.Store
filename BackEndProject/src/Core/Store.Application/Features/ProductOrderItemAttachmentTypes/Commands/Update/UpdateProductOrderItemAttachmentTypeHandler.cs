using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class UpdateProductOrderItemAttachmentTypeHandler
    (IUnitOfWork uow, IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    : IRequestHandler<UpdateProductOrderItemAttachmentTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(UpdateProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var productOrderItemAttachmentType = await productOrderItemAttachmentTypeRepository.FindAsync(request.Id);
        if (productOrderItemAttachmentType is null)
            return ErrorModel.Create("InvalidId");

        productOrderItemAttachmentType.Update(
            request.ProductId,
            request.Description,
            request.OrderItemAttachmentTypeId,
            request.Priority);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
