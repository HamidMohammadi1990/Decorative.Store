using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductPropertyPrices.Queries;

public record GetAllProductPropertyPriceRequest : ContentPolicyRequest<ProductPropertyPrice>, IRequest<OperationResult<PagedResult<GetAllProductPropertyPriceResponse>>>
{
    [JsonConverter(typeof(ProductPropertyNullableEncryptor))]
    public int? ProductPropertyId { get; init; }

    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
