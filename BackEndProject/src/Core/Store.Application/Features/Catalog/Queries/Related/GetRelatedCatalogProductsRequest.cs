using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetRelatedCatalogProductsRequest(string Slug, int Limit = 4)
    : IRequest<OperationResult<GetRelatedCatalogProductsResponse>>;
