using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyStoryComments.Queries;

public record GetAllCompanyStoryCommentRequest : ContentPolicyRequest<CompanyStoryComment>, IRequest<OperationResult<PagedResult<GetAllCompanyStoryCommentResponse>>>
{
    [JsonConverter(typeof(CompanyStoryNullableEncryptor))]
    public int? CompanyStoryId { get; set; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? CreatedByUserId { get; private set; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? ApprovedByUserId { get; private set; }

    public bool? IsApproved { get; init; }
    public PagedRequest Pagination { get; init; } = default!;
}
