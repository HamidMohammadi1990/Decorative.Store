using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductProperties.Commands;

public record DeleteProductPropertyRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int Id { get; init; }
}
