using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public class SearchProductOrderItemAttachmentTypeHandler
    (IProductOrderItemAttachmentTypeRepository productOrderItemAttachmentTypeRepository, IProductOrderItemAttachmentTypeMapperService mapper)
    : IRequestHandler<SearchProductOrderItemAttachmentTypeRequest, OperationResult<PagedResult<SearchProductOrderItemAttachmentTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProductOrderItemAttachmentTypeResponse>>> Handle(SearchProductOrderItemAttachmentTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var items = await productOrderItemAttachmentTypeRepository.SearchAsync(requestModel);
        return mapper.MapToSearch(items);
    }
}
