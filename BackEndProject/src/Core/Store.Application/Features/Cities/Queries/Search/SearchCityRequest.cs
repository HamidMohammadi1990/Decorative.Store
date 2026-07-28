using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Cities.Queries;

public record SearchCityRequest : ContentPolicyRequest<City>, IRequest<OperationResult<PagedResult<SearchCityResponse>>>
{
    [JsonConverter(typeof(ProvinceNullableEncryptor))]
    public int? ProvinceId { get; init; }

    public string? Name { get; init; }
    public string? Slug { get; private set; }
    public PagedRequest Pagination { get; init; } = default!;
}