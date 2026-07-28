using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;

namespace Edition.Application.Features.Properties.Commands;

public record UpdatePropertyRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    [JsonConverter(typeof(PropertyEncryptor))]
    public int? ParentId { get; init; }

    public int Priority { get; init; }

    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int PropertyCategoryId { get; init; }

    public PropertyType PropertyType { get; init; }
    public bool Status { get; init; }
}