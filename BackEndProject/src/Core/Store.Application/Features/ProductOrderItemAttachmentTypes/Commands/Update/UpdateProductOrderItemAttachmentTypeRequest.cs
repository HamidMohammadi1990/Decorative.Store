using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public record UpdateProductOrderItemAttachmentTypeRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(ProductOrderItemAttachmentTypeEncryptor))]
    public int Id { get; init; }
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string? Description { get; init; }
    public int OrderItemAttachmentTypeId { get; init; }
    public int Priority { get; init; }
}
