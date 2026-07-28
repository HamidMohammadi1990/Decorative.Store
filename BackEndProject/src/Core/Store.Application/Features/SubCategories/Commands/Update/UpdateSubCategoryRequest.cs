using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.SubCategories.Commands;

public record UpdateSubCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }

    [JsonConverter(typeof(CategoryEncryptor))]
    public int CategoryId { get; init; }

    public string Title { get; init; } = default!;
    public string Slug { get; init; } = default!;
    public string Code { get; init; } = default!;    
    public bool IsActive { get; init; }
}