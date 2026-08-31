using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyItems.Queries;

public record GetAllPropertyItemRequest : ContentPolicyRequest<PropertyItem>, IRequest<OperationResult<PagedResult<GetAllPropertyItemResponse>>>
{
    public string? Title { get; init; }

    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? PropertyId { get; init; }

    public bool? IsActive { get; init; }
    public string? PropertyTitle { get; init; }

    [JsonConverter(typeof(PropertyCategoryNullableEncryptor))]
    public int? PropertyCategoryId { get; init; }

    public PropertyType? PropertyType { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}