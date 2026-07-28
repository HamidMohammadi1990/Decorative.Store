using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PostTypes.Queries;

public class GetAllPostTypeHandler
    (IPostTypeRepository postTypeRepository, IPostTypeMapperService mapper)
    : IRequestHandler<GetAllPostTypeRequest, OperationResult<PagedResult<GetAllPostTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPostTypeResponse>>> Handle(GetAllPostTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var postTypes = await postTypeRepository.GetAllAsync(requestModel);
        var result = mapper.Map(postTypes);
        return result;
    }
}