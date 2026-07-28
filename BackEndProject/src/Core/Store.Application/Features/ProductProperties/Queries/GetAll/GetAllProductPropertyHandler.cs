using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.ProductProperties.Queries;

public class GetAllProductPropertyHandler
    (IProductPropertyRepository productPropertyRepository, IProductPropertyMapperService mapper)
    : IRequestHandler<GetAllProductPropertyRequest, OperationResult<PagedResult<GetAllProductPropertyResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProductPropertyResponse>>> Handle(GetAllProductPropertyRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var items = await productPropertyRepository.GetAllAsync(requestModel);
        return mapper.Map(items);
    }
}
