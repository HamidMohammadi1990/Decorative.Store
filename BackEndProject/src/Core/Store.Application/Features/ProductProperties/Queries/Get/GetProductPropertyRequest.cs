using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductProperties.Queries;

public record GetProductPropertyRequest : IRequest<OperationResult<GetProductPropertyResponse?>>
{
    [JsonConverter(typeof(ProductPropertyEncryptor))]
    public int Id { get; init; }
}
