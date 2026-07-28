using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Products.Queries;

public record GetProductRequest : IRequest<OperationResult<GetProductResponse?>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int Id { get; init; }
}