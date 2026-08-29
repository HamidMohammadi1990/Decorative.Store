using Microsoft.AspNetCore.Http;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFiles.Commands;

public record CreateProductFileRequest : IRequest<OperationResult<List<CreateProductFileResponse>>>
{
    public List<ProductFileRequest> Files { get; init; } = default!;
}

public record ProductFileRequest
{
    public string ProductId { get; init; } = default!;
    public int LanguageId { get; init; }
    public string Title { get; init; } = default!;
    public IFormFile Image { get; init; } = default!;
    public bool IsIndex { get; init; }
}