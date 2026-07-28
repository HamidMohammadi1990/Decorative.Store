using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Discounts.Queries;

public record GetAllDiscountRequest : ContentPolicyRequest<Discount>, IRequest<PagedResult<GetAllDiscountResponse>>
{
    public string? Code { get; init; }

    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}