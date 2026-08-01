using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Queries;

public class GetAllSubCategoryHandler
    (ISubCategoryRepository subCategoryRepository, ISubCategoryMapperService mapper)
    : IRequestHandler<GetAllSubCategoryRequest, OperationResult<PagedResult<GetAllSubCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllSubCategoryResponse>>> Handle(GetAllSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var subCategories = await subCategoryRepository.GetAllAsync(requestModel, cancellationToken);
        var result = mapper.Map(subCategories);
        return result;
    }
}