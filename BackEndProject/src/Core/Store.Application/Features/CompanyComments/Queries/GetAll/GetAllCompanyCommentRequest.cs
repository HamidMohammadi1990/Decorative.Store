using System.Text.Json.Serialization;
using Edition.Application.Contracts.ContentPolicies;
using Edition.Application.Common.Utilities.Security.Attributes;
using Store.Common.Models;
using Store.Domain.Enums;
using Store.Domain.Dtos.Pagination;
using Store.Domain.Entities;

namespace Edition.Application.Features.CompanyComments.Queries;

public record GetAllCompanyCommentRequest : ContentPolicyRequest<CompanyComment>, IRequest<OperationResult<PagedResult<GetAllCompanyCommentResponse>>>
{
    [JsonConverter(typeof(CompanyNullableEncryptor))]
    public int? CompanyId { get; init; }

    [JsonConverter(typeof(UserNullableEncryptor))]
    public int? UserId { get; set; }

    public CommentStatusType? Status { get; set; }
    public PagedRequest Pagination { get; init; } = default!;
}