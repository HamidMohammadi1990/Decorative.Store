using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public record GetProductOrderItemAttachmentTypeResponse
{
    [JsonConverter(typeof(ProductOrderItemAttachmentTypeEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string? Description { get; init; }
    public int OrderItemAttachmentTypeId { get; init; }
    public int Priority { get; init; }
}
