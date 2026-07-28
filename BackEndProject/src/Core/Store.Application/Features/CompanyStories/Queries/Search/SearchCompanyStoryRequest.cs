using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStories.Queries;

public record SearchCompanyStoryRequest : ContentPolicyRequest<CompanyStory>, IRequest<OperationResult<PagedResult<SearchCompanyStoryResponse>>>
{
    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    public string? Caption { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
