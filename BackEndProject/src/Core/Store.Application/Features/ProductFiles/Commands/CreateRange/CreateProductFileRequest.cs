using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFiles.Commands;

public record CreateProductFileRequest : IRequest<OperationResult<List<CreateProductFileResponse>>>
{
    public List<ProductFileRequest> Files { get; init; } = default!;
}

public record ProductFileRequest
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string Title { get; init; } = default!;
    public IFormFile Image { get; init; } = default!;
    public bool IsIndex { get; init; }
}