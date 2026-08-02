using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.PropertyItemPrices.Queries;

public record GetAllPropertyItemPriceRequest : ContentPolicyRequest<PropertyItemPrice>, IRequest<OperationResult<PagedResult<GetAllPropertyItemPriceResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? PropertyId { get; init; }

    [JsonConverter(typeof(PropertyCategoryNullableEncryptor))]
    public int? PropertyCategoryId { get; init; }

    [JsonConverter(typeof(PropertyItemNullableEncryptor))]
    public int? PropertyItemId { get; init; }

    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
