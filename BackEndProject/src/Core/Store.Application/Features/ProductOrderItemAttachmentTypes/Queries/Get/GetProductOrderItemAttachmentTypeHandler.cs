using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class GetProductOrderItemAttachmentTypeHandler
    (IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository, IProductOrderItemAttachmentTypeMapperService mapper)
    : IRequestHandler<GetProductOrderItemAttachmentTypeRequest, OperationResult<GetProductOrderItemAttachmentTypeResponse?>>
{
    public async Task<OperationResult<GetProductOrderItemAttachmentTypeResponse?>> Handle(GetProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var productOrderItemAttachmentType = await productOrderItemAttachmentTypeRepository.GetAsNoTrackingAsync(request.Id);
        if (productOrderItemAttachmentType is null)
            return ErrorModel.Create("InvalidId");

        return mapper.Map(productOrderItemAttachmentType);
    }
}
