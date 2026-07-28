using Edition.Application.Contracts.ContentPolicies;
using System.Text.Json.Serialization;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.FinancialYears.Queries;

public record GetAllFinancialYearRequest : ContentPolicyRequest<FinancialYear>, IRequest<OperationResult<PagedResult<GetAllFinancialYearResponse>>>
{
    public string? Name { get; init; }
    public bool? IsActive { get; init; } = true;

    [JsonConverter(typeof(CompanyEncryptor))]
    public int? CompanyId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}