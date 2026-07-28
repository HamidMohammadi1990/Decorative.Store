using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Categories.Commands;

public record DeleteCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(CategoryEncryptor))]
    public int Id { get; init; }
}
