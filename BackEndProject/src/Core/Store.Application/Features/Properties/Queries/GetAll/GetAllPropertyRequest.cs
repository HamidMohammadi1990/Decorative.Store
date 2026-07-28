using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Properties.Queries;

public record GetAllPropertyRequest : ContentPolicyRequest<Property>, IRequest<OperationResult<PagedResult<GetAllPropertyResponse>>>
{
    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? ParentId { get; init; }

    public string? Title { get; init; }

    [JsonConverter(typeof(PropertyCategoryNullableEncryptor))]
    public int? PropertyCategoryId { get; init; }

    public PropertyType? PropertyType { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}