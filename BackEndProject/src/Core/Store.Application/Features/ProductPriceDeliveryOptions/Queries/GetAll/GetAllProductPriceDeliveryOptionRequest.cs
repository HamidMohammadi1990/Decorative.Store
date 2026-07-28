using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPriceDeliveryOptions.Queries;

public record GetAllProductPriceDeliveryOptionRequest : ContentPolicyRequest<ProductPriceDeliveryOption>, IRequest<OperationResult<PagedResult<GetAllProductPriceDeliveryOptionResponse>>>
{
    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    [JsonConverter(typeof(ProductPriceNullableEncryptor))]
    public int? ProductPriceId { get; init; }

    [JsonConverter(typeof(DeliveryOptionNullableEncryptor))]
    public int? DeliveryOptionId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public bool? IsActive { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}