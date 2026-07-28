using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Cities.Queries;

public record GetAllCityRequest : ContentPolicyRequest<City>, IRequest<OperationResult<PagedResult<GetAllCityResponse>>>
{
    [JsonConverter(typeof(ProvinceNullableEncryptor))]
    public int? ProvinceId { get; init; }

    public string? Name { get; init; }
    public bool? IsActive { get; init; }
    public string? Slug { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}