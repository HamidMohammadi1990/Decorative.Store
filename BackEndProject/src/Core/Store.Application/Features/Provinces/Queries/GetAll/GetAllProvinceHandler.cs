using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Provinces.Queries;

public class GetAllProvinceHandler
    (IProvinceRepository provinceRepository, IProvinceMapperService mapper)
    : IRequestHandler<GetAllProvinceRequest, OperationResult<PagedResult<GetAllProvinceResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllProvinceResponse>>> Handle(GetAllProvinceRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var provinces = await provinceRepository.GetAllAsync(requestModel);
        var result = mapper.Map(provinces);
        return result;
    }
}