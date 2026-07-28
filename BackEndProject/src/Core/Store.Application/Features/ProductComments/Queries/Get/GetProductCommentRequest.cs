using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Queries;

public record GetProductCommentRequest : IRequest<OperationResult<GetProductCommentResponse?>>
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }
}