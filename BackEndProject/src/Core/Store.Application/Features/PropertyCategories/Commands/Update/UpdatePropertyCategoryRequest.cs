using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyCategories.Commands;

public record UpdatePropertyCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int Id { get; init; }

    public int LanguageId { get; init; }
    public string Code { get; init; } = default!;
    public string Title { get; init; } = default!;
    public bool IsActive { get; init; }
}
