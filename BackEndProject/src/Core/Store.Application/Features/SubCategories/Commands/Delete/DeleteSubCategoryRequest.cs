using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.SubCategories.Commands;

public record DeleteSubCategoryRequest : IRequest<OperationResult>
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }
}