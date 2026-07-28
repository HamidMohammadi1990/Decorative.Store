using Edition.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Sections.Queries;

public class GetAllSectionHandler
    (ISectionRepository repository, ISectionMapperService mapper)
    : IRequestHandler<GetAllSectionRequest, OperationResult<PagedResult<GetAllSectionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllSectionResponse>>> Handle(GetAllSectionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.GetAllAsync(requestModel);
        return mapper.Map(models);
    }
}
