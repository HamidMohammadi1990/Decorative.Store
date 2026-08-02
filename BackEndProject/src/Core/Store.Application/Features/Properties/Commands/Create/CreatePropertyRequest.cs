using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Properties.Commands;

public record CreatePropertyRequest : IRequest<OperationResult<CreatePropertyResponse>>
{
    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string? Description { get; init; }

    [JsonConverter(typeof(PropertyEncryptor))]
    public int? ParentId { get; init; }

    public int Priority { get; init; }

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public PropertyType PropertyType { get; init; }
}
