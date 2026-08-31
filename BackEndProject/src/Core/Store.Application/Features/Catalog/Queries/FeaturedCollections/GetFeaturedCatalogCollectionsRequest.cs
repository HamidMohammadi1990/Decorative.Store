using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetFeaturedCatalogCollectionsRequest
    : IRequest<OperationResult<GetFeaturedCatalogCollectionsResponse>>;
