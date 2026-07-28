using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Enums;
using Store.Domain.Entities;

namespace Edition.Application.Features.Users.Queries;

public record GetAllUserRequest : ContentPolicyRequest<User>, IRequest<OperationResult<PagedResult<GetAllUserResponse>>>
{
    public string? UserName { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public bool? EmailConfirmed { get; init; }
    public string? PhoneNumber { get; init; }
    public bool? PhoneNumberConfirmed { get; init; }
    public bool? LoginPermission { get; init; }
    public GenderType? Gender { get; init; }
    public bool? IsActive { get; init; }
    public RefundMethodType? RefundMethod { get; init; }

    [JsonConverter(typeof(CityNullableEncryptor))]
    public int? CityId { get; init; }

    public string? EconomicCode { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}