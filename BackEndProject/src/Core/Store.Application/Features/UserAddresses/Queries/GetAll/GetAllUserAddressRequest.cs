using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserAddresses.Queries;

public record GetAllUserAddressRequest : ContentPolicyRequest<UserAddress>, IRequest<OperationResult<PagedResult<GetAllUserAddressResponse>>>
{
    public string? Title { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; init; }

    public bool? IsActive { get; init; }
    public string? PostalCode { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}