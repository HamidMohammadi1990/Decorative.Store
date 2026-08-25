using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.SubCategories.Commands;

public record CreateSubCategoryRequest : IRequest<OperationResult<CreateSubCategoryResponse>>
{
    public int LanguageId { get; init; }
    public string Title { get; set; } = default!;
    public string Slug { get; set; } = default!;
    public string Code { get; set; } = default!;

    [JsonConverter(typeof(CategoryEncryptor))]
    public int CategoryId { get; set; }
}
