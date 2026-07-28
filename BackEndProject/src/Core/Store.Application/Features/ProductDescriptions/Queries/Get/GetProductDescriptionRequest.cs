using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public record GetProductDescriptionRequest : IRequest<OperationResult<GetProductDescriptionResponse?>>
{
    [JsonConverter(typeof(ProductDescriptionEncryptor))]
    public int Id { get; init; }
}