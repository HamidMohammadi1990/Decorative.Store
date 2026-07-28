using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Repositories;
using Store.Domain.Dtos.Pagination;

namespace Edition.Application.Features.PageSections.Queries;

public class GetAllPageSectionHandler
    (IPageSectionRepository repository, IPageSectionMapperService mapper)
    : IRequestHandler<GetAllPageSectionRequest, OperationResult<PagedResult<GetAllPageSectionResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPageSectionResponse>>> Handle(GetAllPageSectionRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.GetAllAsync(requestModel);
        return mapper.Map(models);
    }
}
