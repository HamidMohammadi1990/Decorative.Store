using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.UserAddresses.Queries;

public record GetUserAddressesRequest
    : ContentPolicyRequest<UserAddress>,
      IRequest<OperationResult<PagedResult<GetUserAddressesResponse>>>,
      IContentPolicyFilteredRequest
{
    public string? Title { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }

    public string? PostalCode { get; init; }

    public bool? IsActive { get; init; } = true;

    public PagedRequest Pagination { get; init; } = default!;

    ContentPolicyQueryAction? IContentPolicyFilteredRequest.ContentPolicyQueryAction
        => ContentPolicyQueryAction.GetUserAddresses;
}