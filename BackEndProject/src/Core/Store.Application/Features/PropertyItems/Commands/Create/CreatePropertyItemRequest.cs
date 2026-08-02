using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyItems.Commands;

public record CreatePropertyItemRequest : IRequest<OperationResult<CreatePropertyItemResponse>>
{
    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int PropertyId { get; init; }
    public int Priority { get; init; }
}
