using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductDescriptions.Commands;

public record DeleteProductDescriptionRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductDescriptionEncryptor))]
    public int Id { get; init; }
}