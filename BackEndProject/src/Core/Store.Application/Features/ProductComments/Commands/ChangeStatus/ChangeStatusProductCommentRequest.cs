using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductComments.Commands;

public record ChangeStatusProductCommentRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductCommentEncryptor))]
    public int Id { get; init; }

    public bool IsActive { get; init; }
}