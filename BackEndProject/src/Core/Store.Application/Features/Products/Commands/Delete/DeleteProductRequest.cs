using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Products.Commands;

public record DeleteProductRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
}