using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogListingRequest(string Path)
    : IRequest<OperationResult<GetCatalogListingResponse>>;
