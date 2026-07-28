using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStoryLikes.Queries;

public record GetAllCompanyStoryLikeRequest : ContentPolicyRequest<CompanyStoryLike>, IRequest<OperationResult<PagedResult<GetAllCompanyStoryLikeResponse>>>
{
    [JsonConverter(typeof(CompanyStoryNullableEncryptor))]
    public int? CompanyStoryId { get; init; }

    public PagedRequest Pagination { get; init; } = default!;
}
