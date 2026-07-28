using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyCategories.Queries;

public record GetPropertyCategoryRequest : IRequest<OperationResult<GetPropertyCategoryResponse?>>
{
    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int Id { get; init; }
}