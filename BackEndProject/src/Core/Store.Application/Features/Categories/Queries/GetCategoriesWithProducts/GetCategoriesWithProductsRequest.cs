using Store.Common.Models;

namespace Edition.Application.Features.Categories.Queries;

public record GetCategoriesWithProductsRequest : IRequest<OperationResult<List<GetCategoriesWithProductsResponse>>>
{

}