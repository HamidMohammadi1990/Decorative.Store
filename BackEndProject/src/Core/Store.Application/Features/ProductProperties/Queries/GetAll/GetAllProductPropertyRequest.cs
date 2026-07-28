using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductProperties.Queries;

public record GetAllProductPropertyRequest : ContentPolicyRequest<ProductProperty>, IRequest<OperationResult<PagedResult<GetAllProductPropertyResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }
    [JsonConverter(typeof(PropertyNullableEncryptor))]
    public int? PropertyId { get; init; }
    public bool? IsActive { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
