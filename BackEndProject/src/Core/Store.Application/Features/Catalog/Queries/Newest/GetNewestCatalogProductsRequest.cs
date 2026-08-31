using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetNewestCatalogProductsRequest(int Limit = 4)
    : IRequest<OperationResult<GetNewestCatalogProductsResponse>>;
