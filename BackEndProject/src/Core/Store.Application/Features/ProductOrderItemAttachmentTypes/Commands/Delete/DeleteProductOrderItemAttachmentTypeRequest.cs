using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public record DeleteProductOrderItemAttachmentTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductOrderItemAttachmentTypeEncryptor))]
    public int Id { get; init; }
}
