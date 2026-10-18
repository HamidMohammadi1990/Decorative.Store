using Microsoft.AspNetCore.Http;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.ProductFiles.Commands;

public class CreateProductFileRequest : IRequest<OperationResult<List<CreateProductFileResponse>>>
{
    public List<ProductFileRequest> Files { get; set; } = [];
}

public class ProductFileRequest
{
    public string ProductId { get; set; } = default!;
    public int LanguageId { get; set; }
    public string Title { get; set; } = default!;
    public IFormFile Image { get; set; } = default!;
    public bool IsIndex { get; set; }
    public ProductFileKind Kind { get; set; } = ProductFileKind.Gallery;
}