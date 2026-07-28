using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Edition.Application.Contracts.ContentPolicies;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.Companies.Queries;

public record SearchCompanyRequest : ContentPolicyRequest<Company>, IRequest<OperationResult<PagedResult<SearchCompanyResponse>>>
{
    [JsonConverter(typeof(ProductNullableEncryptor))]
    public int? ProductId { get; init; }

    [JsonConverter(typeof(ProvinceNullableEncryptor))]
    public int? ProvinceId { get; set; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }

    public string? Name { get; init; }
    public string? Code { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; set; }

    public string? PostalCode { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}