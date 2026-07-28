using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductProperties.Commands;

public record CreateProductPropertyRequest : IRequest<OperationResult<CreateProductPropertyResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }
    public bool IsActive { get; init; }
}
