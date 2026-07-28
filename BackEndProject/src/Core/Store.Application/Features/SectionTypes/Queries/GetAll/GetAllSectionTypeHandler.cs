using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionTypes.Queries;

public class GetAllSectionTypeHandler
    (ISectionTypeRepository repository, ISectionTypeMapperService mapper)
    : IRequestHandler<GetAllSectionTypeRequest, OperationResult<PagedResult<GetAllSectionTypeResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllSectionTypeResponse>>> Handle(GetAllSectionTypeRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.GetAllAsync(requestModel);
        return mapper.Map(models);
    }
}
