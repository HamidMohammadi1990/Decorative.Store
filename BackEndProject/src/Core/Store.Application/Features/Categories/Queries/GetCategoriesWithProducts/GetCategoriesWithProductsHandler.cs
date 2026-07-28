using Edition.Application.Contracts.Mapping;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Repositories;

namespace Edition.Application.Features.Categories.Queries;

public class GetCategoriesWithProductsHandler
    (ICategoryRepository categoryRepository, ICategoryMapperService mapper)
    : IRequestHandler<GetCategoriesWithProductsRequest, OperationResult<List<GetCategoriesWithProductsResponse>>>
{
    public async Task<OperationResult<List<GetCategoriesWithProductsResponse>>> Handle(GetCategoriesWithProductsRequest request, CancellationToken cancellationToken)
    {
        var categories = await categoryRepository.GetAllWithProductsAsync(ProductFeatureTypeCode.DisplayOnMenu);
        return mapper.Map(categories);
    }
}