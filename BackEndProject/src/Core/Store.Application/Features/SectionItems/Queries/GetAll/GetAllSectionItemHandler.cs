using Store.Domain.Entities;
using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SectionItems.Queries;

public class GetAllSectionItemHandler
    (ISectionItemRepository repository, ISectionItemMapperService mapper)
    : IRequestHandler<GetAllSectionItemRequest, OperationResult<PagedResult<GetAllSectionItemResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllSectionItemResponse>>> Handle(GetAllSectionItemRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var models = await repository.GetAllAsync(requestModel);
        return mapper.Map(models);
    }
}
