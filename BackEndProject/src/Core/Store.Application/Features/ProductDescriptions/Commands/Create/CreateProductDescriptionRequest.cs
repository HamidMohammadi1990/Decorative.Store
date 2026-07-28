using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public record CreateProductDescriptionRequest : IRequest<OperationResult<CreateProductDescriptionResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string Description { get; init; } = default!;
}