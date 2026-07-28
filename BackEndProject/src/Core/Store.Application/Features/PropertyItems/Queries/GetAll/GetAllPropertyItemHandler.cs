using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PropertyItems.Queries;

public class GetAllPropertyItemHandler
    (IPropertyItemRepository propertyItemRepository, IPropertyItemMapperService mapper)
    : IRequestHandler<GetAllPropertyItemRequest, OperationResult<PagedResult<GetAllPropertyItemResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPropertyItemResponse>>> Handle(GetAllPropertyItemRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var propertyItems = await propertyItemRepository.GetAllAsync(requestModel);
        var result = mapper.Map(propertyItems);
        return result;
    }
}