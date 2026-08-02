using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Properties.Queries;

public class GetAllPropertyHandler
    (IPropertyRepository propertyRepository, IPropertyMapperService mapper)
    : IRequestHandler<GetAllPropertyRequest, OperationResult<PagedResult<GetAllPropertyResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPropertyResponse>>> Handle(GetAllPropertyRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var properties = await propertyRepository.GetAllAsync(requestModel, cancellationToken);
        var result = mapper.Map(properties);
        return result;
    }
}