using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;

namespace Edition.Application.Features.SubCategories.Queries;

public record GetSubCategoryRequest : IRequest<OperationResult<GetSubCategoryResponse?>>
{
    [JsonConverter(typeof(SubCategoryEncryptor))]
    public int Id { get; init; }
}