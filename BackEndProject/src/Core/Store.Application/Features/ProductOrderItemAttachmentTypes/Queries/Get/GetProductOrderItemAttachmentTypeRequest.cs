using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.ProductOrderItemAttachmentTypes.Queries;

public record GetProductOrderItemAttachmentTypeRequest : IRequest<OperationResult<GetProductOrderItemAttachmentTypeResponse?>>
{
    [JsonConverter(typeof(ProductOrderItemAttachmentTypeEncryptor))]
    public int Id { get; init; }
}
