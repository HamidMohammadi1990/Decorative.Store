using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFiles.Commands;

public record DeleteProductFileRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductFileEncryptor))]
    public int Id { get; init; }
}