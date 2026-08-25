using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.SubCategories.Queries;

public record GetAllSubCategoryRequest : ContentPolicyRequest<SubCategory>, IRequest<OperationResult<PagedResult<GetAllSubCategoryResponse>>>
{
    public string? Title { get; init; }
    public string? Slug { get; init; }
    public string? Code { get; init; }

    [JsonConverter(typeof(CategoryNullableEncryptor))]
    public int? CategoryId { get; init; }

    public string? CategoryTitle { get; init; }
    public string? CategoryCode { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
