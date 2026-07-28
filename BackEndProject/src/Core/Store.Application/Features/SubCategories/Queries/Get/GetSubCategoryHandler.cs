using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Repositories;

namespace Edition.Application.Features.SubCategories.Queries;

public class GetSubCategoryHandler
    (ISubCategoryRepository subCategoryRepository, ISubCategoryMapperService mapper)
    : IRequestHandler<GetSubCategoryRequest, OperationResult<GetSubCategoryResponse?>>
{
    public async Task<OperationResult<GetSubCategoryResponse?>> Handle(GetSubCategoryRequest request, CancellationToken cancellationToken)
    {
        var subCategory = await subCategoryRepository.GetAsNoTrackingAsync(request.Id);
        if (subCategory is null)
            return ErrorModel.Create("InvalidId");

        var result = mapper.Map(subCategory);
        return result;
    }
}