using Edition.Application.Contracts.Mapping;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Extensions;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Queries;

public class GetAllPropertyCategoryHandler
    (IPropertyCategoryRepository propertyCategoryRepository, IPropertyCategoryMapperService mapper)
    : IRequestHandler<GetAllPropertyCategoryRequest, OperationResult<PagedResult<GetAllPropertyCategoryResponse>>>
{
    public async Task<OperationResult<PagedResult<GetAllPropertyCategoryResponse>>> Handle(GetAllPropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var requestModel = mapper.Map(request);
        var propertyCategories = await propertyCategoryRepository.GetAllAsync(requestModel);
        var result = mapper.Map(propertyCategories);
        return result;
    }
}