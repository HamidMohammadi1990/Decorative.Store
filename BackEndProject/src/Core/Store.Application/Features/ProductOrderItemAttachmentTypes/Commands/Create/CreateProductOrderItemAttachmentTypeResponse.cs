using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public record CreateProductOrderItemAttachmentTypeResponse
{
    [JsonConverter(typeof(ProductOrderItemAttachmentTypeEncryptor))]
    public int Id { get; init; }
}
