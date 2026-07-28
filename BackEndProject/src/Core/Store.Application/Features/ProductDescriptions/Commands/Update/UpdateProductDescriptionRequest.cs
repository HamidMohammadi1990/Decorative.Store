using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public record UpdateProductDescriptionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductDescriptionEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }

    public string Description { get; init; } = default!;
}