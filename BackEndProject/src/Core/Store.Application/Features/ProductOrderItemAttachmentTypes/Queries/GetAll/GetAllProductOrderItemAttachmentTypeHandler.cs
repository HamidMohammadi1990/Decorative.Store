using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class GetAllProductOrderItemAttachmentTypeHandler
    (IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository, IProductOrderItemAttachmentTypeMapperService mapper)
    : IRequestHandler<GetAllProductOrderItemAttachmentTypeRequest, OperationResult<PagedResult<GetAllProductOrderItemAttachmentTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductOrderItemAttachmentTypeResponse>>> Handle(GetAllProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var items = await productOrderItemAttachmentTypeRepository.GetAllAsync(requestModel);
        return mapper.Map(items);
    }
}
