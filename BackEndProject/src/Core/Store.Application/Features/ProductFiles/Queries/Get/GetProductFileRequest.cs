using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductFiles.Queries;

public record GetProductFileRequest : IRequest<OperationResult<GetProductFileResponse?>>
{
    [JsonConverter(typeof(ProductFileEncryptor))]
    public int Id { get; init; }
}