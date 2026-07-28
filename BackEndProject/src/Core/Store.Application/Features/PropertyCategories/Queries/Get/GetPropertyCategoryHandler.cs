using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.PropertyCategories.Queries;

public class GetPropertyCategoryHandler
    (IPropertyCategoryRepository propertyCategoryRepository, IPropertyCategoryMapperService mapper)
    : IRequestHandler<GetPropertyCategoryRequest, OperationResult<GetPropertyCategoryResponse?>>
{
    public async Task<OperationResult<GetPropertyCategoryResponse?>> Handle(GetPropertyCategoryRequest request, CancellationToken cancellationToken)
    {
        var propertyCategory = await propertyCategoryRepository.GetAsNoTrackingAsync(request.Id);
        if (propertyCategory is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(propertyCategory);
        return result;
    }
}