using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Commands;

public record CreateProductOrderItemAttachmentTypeRequest : IRequest<OperationResult<CreateProductOrderItemAttachmentTypeResponse>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public string? Description { get; init; }
    public int OrderItemAttachmentTypeId { get; init; }
    public int Priority { get; init; }
}
