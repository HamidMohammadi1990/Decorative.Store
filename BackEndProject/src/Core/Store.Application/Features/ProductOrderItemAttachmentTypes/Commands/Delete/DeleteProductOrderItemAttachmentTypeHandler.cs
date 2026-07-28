using Edition.Application.Contracts.Persistence;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public class DeleteProductOrderItemAttachmentTypeHandler
    (IUnitOfWork uow, IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository)
    : IRequestHandler<DeleteProductOrderItemAttachmentTypeRequest, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var productOrderItemAttachmentType = await productOrderItemAttachmentTypeRepository.FindAsync(request.Id);
        if (productOrderItemAttachmentType is null)
            return ErrorModel.Create("InvalidId");

        productOrderItemAttachmentTypeRepository.Remove(productOrderItemAttachmentType);

        var saveChangesResult = await uow.SaveChangesAsync(cancellationToken);
        if (!saveChangesResult.IsSuccess)
            return saveChangesResult;

        return OperationResult.Success();
    }
}
