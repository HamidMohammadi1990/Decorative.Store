using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyPosDevices.Queries;

public record GetAllCompanyPosDeviceRequest : ContentPolicyRequest<CompanyPosDevice>, IRequest<OperationResult<PagedResult<GetAllCompanyPosDeviceResponse>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; } = true;

    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    [JsonConverter(typeof(BankNullableEncryptor))]
    public int? BankId { get; init; }

    public string? IP { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}