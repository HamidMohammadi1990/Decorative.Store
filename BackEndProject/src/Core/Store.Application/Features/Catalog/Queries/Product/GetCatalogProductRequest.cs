using Store.Common.Models;

namespace Edition.Application.Features.Catalog.Queries;

public record GetCatalogProductRequest(string Slug)
    : IRequest<OperationResult<GetCatalogProductResponse>>;
