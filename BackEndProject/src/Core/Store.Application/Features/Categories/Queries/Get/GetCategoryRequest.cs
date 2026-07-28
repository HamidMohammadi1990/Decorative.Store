using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.Categories.Queries;

public record GetCategoryRequest : IRequest<OperationResult<GetCategoryResponse?>>
{
    [JsonConverter(typeof(CategoryEncryptor))]
    public int Id { get; init; }
}