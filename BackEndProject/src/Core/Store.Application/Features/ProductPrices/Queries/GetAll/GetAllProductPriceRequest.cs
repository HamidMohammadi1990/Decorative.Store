using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPrices.Queries;

public record GetAllProductPriceRequest : ContentPolicyRequest<ProductPrice>, IRequest<OperationResult<PagedResult<GetAllProductPriceResponse>>>
{
    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}