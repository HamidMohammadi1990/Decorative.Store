using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Properties.Commands;

public record CreatePropertyRequest : IRequest<OperationResult<CreatePropertyResponse>>
{
    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int? ParentId { get; init; }

    public int Priority { get; init; }

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public string? Description { get; set; }

    public PropertyType PropertyType { get; init; }
}