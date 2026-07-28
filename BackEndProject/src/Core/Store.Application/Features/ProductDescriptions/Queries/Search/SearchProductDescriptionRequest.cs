using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.ProductDescriptions.Queries;

public record SearchProductDescriptionRequest : ContentPolicyRequest<ProductDescription>, IRequest<OperationResult<PagedResult<SearchProductDescriptionResponse>>>
{
    [JsonConverter(typeof(ProductEncryptor))]
    public int ProductId { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}