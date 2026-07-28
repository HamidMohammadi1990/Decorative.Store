using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Provinces.Queries;

public class SearchProvinceHandler
    (IProvinceRepository provinceRepository, IProvinceMapperService mapper)
    : IRequestHandler<SearchProvinceRequest, OperationResult<PagedResult<SearchProvinceResponse>>>
{
    public async Task<OperationResult<PagedResult<SearchProvinceResponse>>> Handle(SearchProvinceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var provinces = await provinceRepository.SearchAsync(requestModel);
        var result = mapper.Map(provinces);
        return result;
    }
}