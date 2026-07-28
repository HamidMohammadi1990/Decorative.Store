using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStories.Queries;

public record GetAllCompanyStoryRequest : ContentPolicyRequest<CompanyStory>, IRequest<OperationResult<PagedResult<GetAllCompanyStoryResponse>>>
{
    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? CreatedByUserId { get; init; }

    public bool? IsActive { get; init; }
    public string? Caption { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
