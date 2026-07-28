using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record SearchCompanyStoryCommentRequest : ContentPolicyRequest<CompanyStoryComment>, IRequest<OperationResult<PagedResult<SearchCompanyStoryCommentResponse>>>
{
    [JsonConverter(typeof(CompanyStoryEncryptor))]
    public int CompanyStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
