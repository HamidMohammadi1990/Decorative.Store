using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogSearchRequest(string Query, int Limit = 8)
    : IRequest<OperationResult<GetCatalogSearchResponse>>;
