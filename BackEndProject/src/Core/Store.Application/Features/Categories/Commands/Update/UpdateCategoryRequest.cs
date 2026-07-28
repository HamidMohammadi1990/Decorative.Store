using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Categories.Commands;

public record UpdateCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CategoryEncryptor))]
    public int Id { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;
    public bool IsActive { get; init; }
}
