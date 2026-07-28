using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PostTypes.Queries;

public class SearchPostTypeHandler
    (IPostTypeRepository postTypeRepository, IPostTypeMapperService mapper)
    : IRequestHandler<SearchPostTypeRequest, OperationResult<PagedResult<SearchPostTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchPostTypeResponse>>> Handle(SearchPostTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var postTypes = await postTypeRepository.SearchAsync(requestModel);
        var result = mapper.Map(postTypes);
        return result;
    }
}