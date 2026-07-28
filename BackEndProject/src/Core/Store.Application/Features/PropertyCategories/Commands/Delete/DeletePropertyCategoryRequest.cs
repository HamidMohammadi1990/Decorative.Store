using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.PropertyCategories.Commands;

public record DeletePropertyCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(PropertyCategoryEncryptor))]
    public int Id { get; init; }
}